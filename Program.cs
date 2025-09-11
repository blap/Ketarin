// Ketarin - Copyright (C) 2008  Canneverbe Limited
// The icons used within the application are *NOT* licensed under the GNU General Public License.

// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using CDBurnerXP;
using Ketarin.Forms;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace Ketarin
{
    internal static class Program
    {
        private static NotifyIcon? m_Icon;

        [STAThread]
        private static void Main(string[] args)
        {
            // Check if we're running in test mode
            if (args.Length > 0 && args[0] == "--test")
            {
                GitHubServices.RunIntegrationTests();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Set an error handler (just a message box) for unexpected exceptions in threads
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Parse command line arguments
            CommandlineArguments arguments = new CommandlineArguments(args);

            // Is a database path set per command line?
            if (!string.IsNullOrEmpty(arguments["database"]))
            {
                DbManager.DatabasePath = arguments["database"];
            }

            // Initialise database
            try
            {
                DbManager.CreateOrUpgradeDatabase();

                WebRequest.DefaultWebProxy = DbManager.Proxy;
                if (Settings.GetValue("AuthorGuid") == null)
                {
                    Settings.SetValue("AuthorGuid", Guid.NewGuid().ToString("B"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create or load the database file: " + ex.Message);
                return;
            }

            // Initialisation of protocols.
            WebRequest.RegisterPrefix("sf", new ScpWebRequestCreator());
            WebRequest.RegisterPrefix("httpx", new HttpxRequestCreator());

            // Do not try using SSL3 by default since some websites make SSL3 requests fail.
            // Note: ServicePointManager is obsolete in .NET 9, but we're keeping it for compatibility
#pragma warning disable SYSLIB0014
            ServicePointManager.SecurityProtocol = Updater.DefaultHttpProtocols;
#pragma warning restore SYSLIB0014

            // Either run silently on command line...
            if (arguments["silent"] != null)
            {
                Kernel32.ManagedAttachConsole(Kernel32.ATTACH_PARENT_PROCESS);

                ApplicationJob[] jobs = DbManager.GetJobs();
                
                // Filter by application name and category.
                string categoryFilter = arguments["category"];
                if (!string.IsNullOrEmpty(categoryFilter))
                {
                    jobs = jobs.Where(x => string.Equals(x.Category, categoryFilter, StringComparison.OrdinalIgnoreCase)).ToArray();
                }
                
                string appFilter = arguments["appname"];
                if (!string.IsNullOrEmpty(appFilter))
                {
                    jobs = jobs.Where(x => LikeOperator.LikeString(x.Name, appFilter, Microsoft.VisualBasic.CompareMethod.Text)).ToArray();
                }

                Updater updater = new Updater();
                updater.StatusChanged += updater_StatusChanged;
                updater.ProgressChanged += updater_ProgressChanged;
                updater.BeginUpdate(jobs, false, false);

                if (arguments["notify"] != null)
                {
                    m_Icon = new NotifyIcon
                    {
                        Icon = System.Drawing.Icon.FromHandle(Properties.Resources.Restart.GetHicon()),
                        Text = "Ketarin is working...",
                        Visible = true
                    };
                }

                while (updater.IsBusy)
                {
                    Thread.Sleep(1000);
                }

                m_Icon?.Dispose();

                Kernel32.FreeConsole();
            }
            // ...perform database operations...
            else if (arguments["update"] != null && arguments["appguid"] != null)
            {
                // Update properties of an application in the database
                string? appGuid = arguments["appguid"];
                ApplicationJob? job = string.IsNullOrEmpty(appGuid) ? null : DbManager.GetJob(new Guid(appGuid ?? string.Empty));
                if (job == null) return;

                if (arguments["PreviousLocation"] != null)
                {
                    job.PreviousLocation = arguments["PreviousLocation"];
                }

                job.Save();
            }
            else if (arguments["export"] != null)
            {
                ApplicationJob[] jobs = DbManager.GetJobs();
                string exportedXml = ApplicationJob.GetXml(jobs, false, System.Text.Encoding.UTF8);
                try
                {
                    File.WriteAllText(arguments["export"], exportedXml, System.Text.Encoding.UTF8); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not export to the specified location: " + ex.Message);
                }
            }
            else if (arguments["import"] != null)
            {
                string sourceFile = arguments["import"];
                try
                {
                    if (arguments["clear"] != null)
                    {
                        ApplicationJob.DeleteAll();
                    }

                    ApplicationJob.ImportFromXml(sourceFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not import from the specified location: " + ex.Message);
                }
            }
            else if (arguments["install"] != null)
            {
                try
                {
                    // Install all applications in the given XML
                    ApplicationJob[] appsToInstall;
                    string path = arguments["install"];
                    if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
                    {
                        using (WebClient client = new WebClient())
                        {
                            appsToInstall = ApplicationJob.ImportFromXmlString(client.DownloadString(path), false);
                        }
                    }
                    else
                    {
                        appsToInstall = ApplicationJob.ImportFromXml(path);
                    }

                    InstallingApplicationsDialog dialog = new InstallingApplicationsDialog
                    {
                        Applications = appsToInstall,
                        ShowIcon = true,
                        ShowInTaskbar = true,
                        AutoClose = (arguments["exit"] != null)
                    };
                    Application.Run(dialog);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Setup cannot be started: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // ...or show the GUI
            else
            {
                MainForm form = new MainForm();
                Application.Run(form);
            }
        }

        private static void updater_ProgressChanged(object? sender, Updater.JobProgressChangedEventArgs e)
        {
            // This is intentionally left blank
        }

        private static void updater_StatusChanged(object? sender, Updater.JobStatusChangedEventArgs e)
        {
            switch (e.NewStatus)
            {
                case Updater.Status.UpdateSuccessful:
                    Console.WriteLine("Update successful: " + e.ApplicationJob.Name);
                    break;
                case Updater.Status.Failure:
                    Console.WriteLine("Update failed: " + e.ApplicationJob.Name);
                    break;
                case Updater.Status.UpdateAvailable:
                    Console.WriteLine("Update available: " + e.ApplicationJob.Name);
                    break;
                case Updater.Status.NoUpdate:
                    Console.WriteLine("No update: " + e.ApplicationJob.Name);
                    break;
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            MessageBox.Show("An unhandled exception occurred: " + ex.Message + "\n\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}