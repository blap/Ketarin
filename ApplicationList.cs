using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Linq; // Added for LINQ methods like Any() and Select()
using CDBurnerXP.Controls;
using CDBurnerXP;
using CDBurnerXP.IO;
using Ketarin.Properties;

namespace Ketarin
{
    /// <summary>
    /// Represents a list of applications.
    /// </summary>
    public class ApplicationList
    {
        #region ApplicationBindingList

        /// <summary>
        /// Keeps the list in a consitent state (no duplicate applications per list allowed).
        /// </summary>
        public class ApplicationBindingList : BindingList<ApplicationJob>
        {
            protected override void InsertItem(int index, ApplicationJob item)
            {
                // Do not allow duplicate apps in a list
                if (this.Any(app => app == item))
                {
                    return;
                }

                base.InsertItem(index, item);
            }

            internal void AddRange(ApplicationJob[] applicationJobs)
            {
                foreach (ApplicationJob job in applicationJobs)
                {
                    this.Add(job);
                }
            }

            internal ApplicationJob[] ToArray()
            {
                ApplicationJob[] jobs = new ApplicationJob[this.Count];
                for (int i = 0; i < this.Count; i++)
                {
                    jobs[i] = this[i];
                }
                return jobs;
            }
        }

        #endregion

        private readonly bool isPredefined;
        private readonly ApplicationBindingList applications = new ApplicationBindingList();

        /// <summary>
        /// Gets or sets the GUID of the list.
        /// </summary>
        public Guid Guid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets if the list is predefined (cannot be changed by the user).
        /// </summary>
        public bool IsPredefined
        {
            get { return isPredefined; }
        }

        /// <summary>
        /// Gets the list of applications that are on this list.
        /// </summary>
        public ApplicationBindingList Applications
        {
            get
            {
                return this.applications;
            }
        }

        public string Name
        {
            get;
            set;
        } = string.Empty;

        public string Category
        {
            get;
            set;
        } = string.Empty;

        /// <summary>
        /// Returns a comma separated list of all application names.
        /// </summary>
        public string ApplicationNames
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (ApplicationJob job in this.Applications)
                {
                    sb.Append(job.Name);
                    sb.Append(", ");
                }
                return sb.ToString().TrimEnd(',', ' ');
            }
        }

        internal ApplicationList()
        {
            this.Name = string.Empty;
            this.Category = string.Empty;
        }

        public ApplicationList(string name, bool isPredefined)
        {
            this.Name = name ?? string.Empty;
            this.Category = string.Empty;
            this.isPredefined = isPredefined;
        }

        /// <summary>
        /// Generates an icon based on the contained applications.
        /// </summary>
        internal Image GetIcon()
        {
            // Check for application icons that can be extracted
            List<string> iconPaths = this.Applications.Select(app => app.CurrentLocation).Where(location => !string.IsNullOrEmpty(location)).ToList();

            if (iconPaths.Count == 0)
            {
                return Resources.Setup32;
            }
            else
            {
                Bitmap bitmap = new Bitmap(32, 32, PixelFormat.Format32bppArgb);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    if (iconPaths.Count == 1)
                    {
                        using (Icon programIcon = Microsoft.Win32.IconReader.GetFileIcon(iconPaths[0], Microsoft.Win32.IconReader.IconSize.Large, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), new Point(0, 0));
                        }
                    }
                    else if (iconPaths.Count == 2)
                    {
                        using (Icon programIcon = Microsoft.Win32.IconReader.GetFileIcon(iconPaths[0], Microsoft.Win32.IconReader.IconSize.Large, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 0, 0, 22, 22);
                        }
                        using (Icon programIcon = Microsoft.Win32.IconReader.GetFileIcon(iconPaths[1], Microsoft.Win32.IconReader.IconSize.Large, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 9, 9, 22, 22);
                        }
                    }
                    else if (iconPaths.Count == 3)
                    {
                        using (Icon programIcon = Microsoft.Win32.IconReader.GetFileIcon(iconPaths[0], Microsoft.Win32.IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 0, 0, 16, 16);
                        }
                        using (Icon programIcon = Microsoft.Win32.IconReader.GetFileIcon(iconPaths[1], Microsoft.Win32.IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 8, 8, 16, 16);
                        }
                        using (Icon programIcon = Microsoft.Win32.IconReader.GetFileIcon(iconPaths[2], Microsoft.Win32.IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 16, 16, 16, 16);
                        }
                    }
                    else
                    {
                        using (Icon programIcon = Microsoft.Win32.IconReader.GetFileIcon(iconPaths[0], Microsoft.Win32.IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 0, 0, 16, 16);
                        }
                        using (Icon programIcon = Microsoft.Win32.IconReader.GetFileIcon(iconPaths[1], Microsoft.Win32.IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 16, 0, 16, 16);
                        }
                        using (Icon programIcon = Microsoft.Win32.IconReader.GetFileIcon(iconPaths[2], Microsoft.Win32.IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 0, 16, 16, 16);
                        }
                        using (Icon programIcon = Microsoft.Win32.IconReader.GetFileIcon(iconPaths[3], Microsoft.Win32.IconReader.IconSize.Small, false))
                        {
                            g.DrawImage(programIcon.ToBitmap(), 16, 16, 16, 16);
                        }
                    }
                }

                return bitmap;
            }
        }

        /// <summary>
        /// Saves the application list into the database.
        /// </summary>
        public void Save()
        {
            // For JSON database, we'll need to implement a different approach
            // For now, we'll just set the GUID if it's empty
            if (this.Guid == Guid.Empty)
            {
                this.Guid = Guid.NewGuid();
            }

            // In a JSON-based approach, we would save the list to a JSON file
            // For now, we'll just leave this as a placeholder
        }

        /// <summary>
        /// Reads the database fields from a data reader.
        /// </summary>
        internal void Hydrate(IDataReader reader)
        {
            this.Guid = new Guid(reader["ListGuid"] as string ?? string.Empty);
            this.Name = reader["Name"] as string ?? string.Empty;
        }

        /// <summary>
        /// Deletes the list from the database.
        /// </summary>
        public void Delete()
        {
            if (this.isPredefined) return;

            // For JSON database, we'll need to implement a different approach
            // For now, we'll just leave this as a placeholder
        }
    }
}
