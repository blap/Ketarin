using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using CDBurnerXP;
using CDBurnerXP.IO;
using System.Net;
using Ketarin.Database;
using static Ketarin.ApplicationJob;

namespace Ketarin
{
    /// <summary>
    /// This class contains a collection of functions
    /// for reading from the database.
    /// </summary>
    internal class DbManager
    {
        #region SettingsProvider

        /// <summary>
        /// Saves all of Ketarin's settings to the database to be more self contained.
        /// </summary>
        public class SettingsProvider : ISettingsProvider
        {
            #region ISettingsProvider Member

            private string GetPath(string[] path)
            {
                return String.Join("/", path).Trim('/');
            }

            public object GetValue(params string[] path)
            {
                string pathStr = this.GetPath(path);
                return JsonDbManager.GetSetting(pathStr);
            }

            public void SetValueRaw(string value, string path, IDbTransaction transaction)
            {
                JsonDbManager.SetSetting(path, value);
            }

            public void SetValue(string? value, params string[] path)
            {
                string pathStr = this.GetPath(path);
                JsonDbManager.SetSetting(pathStr, value);
            }

            #endregion
        }

        #endregion

        private static string? m_DatabasePath;

        /// <summary>
        /// Sets a predefined database path if necessary.
        /// </summary>
        public static string DatabasePath
        {
            set { m_DatabasePath = (value == null) ? null : Path.GetFullPath(value); }
            get { return m_DatabasePath; }
        }

        /// <summary>
        /// Builds a new proxy object from the settings.
        /// Returns null if no valid proxy exists.
        /// </summary>
        public static WebProxy Proxy
        {
            get
            {
                return JsonDbManager.Proxy;
            }
        }

        /// <summary>
        /// Initializes the database manager
        /// </summary>
        public static void Initialize()
        {
            JsonDbManager.DatabasePath = m_DatabasePath;
            JsonDbManager.Initialize();
            Settings.Provider = new SettingsProvider();
        }

        /// <summary>
        /// Creates or upgrades the database if necessary.
        /// </summary>
        public static void CreateOrUpgradeDatabase()
        {
            Initialize();
        }

        /// <summary>
        /// Gets all application jobs from the database
        /// </summary>
        public static ApplicationJob[] GetJobs()
        {
            JsonApplicationJob[] jsonJobs = JsonDbManager.GetJobs();
            ApplicationJob[] jobs = new ApplicationJob[jsonJobs.Length];
            
            for (int i = 0; i < jsonJobs.Length; i++)
            {
                jobs[i] = ConvertFromJson(jsonJobs[i]);
            }
            
            return jobs;
        }

        /// <summary>
        /// Gets a specific application job by GUID
        /// </summary>
        public static ApplicationJob GetJob(Guid guid)
        {
            JsonApplicationJob[] jsonJobs = JsonDbManager.GetJobs();
            
            foreach (JsonApplicationJob jsonJob in jsonJobs)
            {
                if (Guid.Parse(jsonJob.JobGuid) == guid)
                {
                    return ConvertFromJson(jsonJob);
                }
            }
            
            return null;
        }

        /// <summary>
        /// Saves an application job to the database
        /// </summary>
        public static void SaveJob(ApplicationJob job)
        {
            JsonApplicationJob jsonJob = ConvertToJson(job);
            JsonDbManager.SaveJob(jsonJob);
        }

        /// <summary>
        /// Deletes an application job from the database
        /// </summary>
        public static void DeleteJob(Guid guid)
        {
            JsonDbManager.DeleteJob(guid.ToString());
        }

        /// <summary>
        /// Deletes all application jobs from the database
        /// </summary>
        public static void DeleteAllJobs()
        {
            // This would need to be implemented in JsonDbManager
            // For now, we'll just clear all jobs
            JsonApplicationJob[] jobs = JsonDbManager.GetJobs();
            foreach (JsonApplicationJob job in jobs)
            {
                JsonDbManager.DeleteJob(job.JobGuid);
            }
        }

        /// <summary>
        /// Gets snippets from the database
        /// </summary>
        public static Snippet[] GetSnippets()
        {
            JsonSnippet[] jsonSnippets = JsonDbManager.GetSnippets();
            Snippet[] snippets = new Snippet[jsonSnippets.Length];
            
            for (int i = 0; i < jsonSnippets.Length; i++)
            {
                snippets[i] = ConvertFromJson(jsonSnippets[i]);
            }
            
            return snippets;
        }

        /// <summary>
        /// Gets all variable URLs from the database
        /// </summary>
        public static string[] GetVariableUrls()
        {
            // For now, return an empty array since we don't have this functionality
            return new string[0];
        }

        /// <summary>
        /// Gets the most used variable names from the database
        /// </summary>
        public static string[] GetMostUsedVariableNames()
        {
            // For now, return an empty array since we don't have this functionality
            return new string[0];
        }

        /// <summary>
        /// Gets categories from the database
        /// </summary>
        public static string[] GetCategories()
        {
            // For now, return an empty array since we don't have this functionality
            return new string[0];
        }

        /// <summary>
        /// Gets all setup lists from the database
        /// </summary>
        public static ApplicationList[] GetSetupLists()
        {
            // For now, return an empty array since we don't have this functionality
            return new ApplicationList[0];
        }

        /// <summary>
        /// Gets all settings from the database
        /// </summary>
        public static Dictionary<string, string> GetSettings()
        {
            // For now, return an empty dictionary since we don't have this functionality
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Sets all settings in the database
        /// </summary>
        public static void SetSettings(Dictionary<string, string> settings)
        {
            // For now, do nothing since we don't have this functionality
        }

        /// <summary>
        /// Saves a snippet to the database
        /// </summary>
        public static void SaveSnippet(Snippet snippet)
        {
            JsonSnippet jsonSnippet = ConvertToJson(snippet);
            JsonDbManager.SaveSnippet(jsonSnippet);
        }

        /// <summary>
        /// Formats a GUID for database storage
        /// </summary>
        public static string FormatGuid(Guid guid)
        {
            return guid.ToString();
        }

        /// <summary>
        /// Formats a GUID for database storage
        /// </summary>
        public static string FormatGuid(string guid)
        {
            return guid;
        }

        /// <summary>
        /// Checks if an application with the given GUID exists in the database
        /// </summary>
        public static bool ApplicationExists(Guid guid)
        {
            return JsonDbManager.ApplicationExists(guid.ToString());
        }

        /// <summary>
        /// Gets the value of the private m_PreviousRelativeLocation field
        /// </summary>
        private static string GetPreviousRelativeLocation(ApplicationJob job)
        {
            var field = typeof(ApplicationJob).GetField("m_PreviousRelativeLocation", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                return field.GetValue(job) as string;
            }
            return string.Empty;
        }

        /// <summary>
        /// Converts a JSON application job to an ApplicationJob
        /// </summary>
        private static ApplicationJob ConvertFromJson(JsonApplicationJob jsonJob)
        {
            ApplicationJob job = new ApplicationJob
            {
                Name = jsonJob.ApplicationName,
                TargetPath = jsonJob.TargetPath,
                FixedDownloadUrl = jsonJob.FixedDownloadUrl,
                FileHippoId = jsonJob.FileHippoId,
                DateAdded = jsonJob.DateAdded,
                LastUpdated = jsonJob.LastUpdated,
                DownloadDate = jsonJob.DownloadDate,
                PreviousLocation = jsonJob.PreviousLocation,
                ExecuteCommand = jsonJob.ExecuteCommand,
                HttpReferer = jsonJob.HttpReferer,
                VariableChangeIndicator = jsonJob.VariableChangeIndicator,
                DeletePreviousFile = jsonJob.DeletePreviousFile != 0,
                DownloadBeta = (DownloadBetaType)jsonJob.DownloadBeta,
                CanBeShared = jsonJob.CanBeShared != 0,
                ShareApplication = jsonJob.ShareApplication != 0,
                DownloadSourceType = (SourceType)jsonJob.SourceType,
                Enabled = jsonJob.IsEnabled != 0,
                ExecuteCommandType = (Ketarin.ScriptType)Enum.Parse(typeof(Ketarin.ScriptType), jsonJob.ExecuteCommandType ?? string.Empty),
                ExecutePreCommand = jsonJob.ExecutePreCommand,
                ExecutePreCommandType = (Ketarin.ScriptType)Enum.Parse(typeof(Ketarin.ScriptType), jsonJob.ExecutePreCommandType ?? string.Empty),
                SourceTemplate = jsonJob.SourceTemplate,
                Category = jsonJob.Category,
                ExclusiveDownload = jsonJob.ExclusiveDownload != 0,
                CheckForUpdatesOnly = jsonJob.CheckForUpdateOnly != 0,
                UserNotes = jsonJob.UserNotes,
                WebsiteUrl = jsonJob.WebsiteUrl,
                UserAgent = jsonJob.UserAgent,
                HashType = (HashType)jsonJob.HashType,
                HashVariable = jsonJob.HashVariable,
                NumberOfRevisions = jsonJob.NumberOfRevisions,
                LastFileSize = jsonJob.LastFileSize,
                LastFileDate = jsonJob.LastFileDate,
                IgnoreFileInformation = jsonJob.IgnoreFileInformation != 0
            };

            // Set the GUID
            job.Guid = string.IsNullOrEmpty(jsonJob.JobGuid) ? Guid.Empty : Guid.Parse(jsonJob.JobGuid);

            // Set the private m_PreviousRelativeLocation field using reflection
            if (!string.IsNullOrEmpty(jsonJob.PreviousRelativeLocation))
            {
                var field = typeof(ApplicationJob).GetField("m_PreviousRelativeLocation", BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    field.SetValue(job, jsonJob.PreviousRelativeLocation);
                }
            }

            return job;
        }

        /// <summary>
        /// Converts an ApplicationJob to a JSON application job
        /// </summary>
        private static JsonApplicationJob ConvertToJson(ApplicationJob job)
        {
            JsonApplicationJob jsonJob = new JsonApplicationJob
            {
                JobGuid = job.Guid.ToString(),
                ApplicationName = job.Name,
                TargetPath = job.TargetPath,
                FixedDownloadUrl = job.FixedDownloadUrl,
                FileHippoId = job.FileHippoId,
                DateAdded = job.DateAdded,
                LastUpdated = job.LastUpdated,
                DownloadDate = job.DownloadDate,
                PreviousLocation = job.PreviousLocation,
                ExecuteCommand = job.ExecuteCommand,
                HttpReferer = job.HttpReferer,
                VariableChangeIndicator = job.VariableChangeIndicator,
                DeletePreviousFile = job.DeletePreviousFile ? 1 : 0,
                DownloadBeta = (int)job.DownloadBeta,
                CanBeShared = job.CanBeShared ? 1 : 0,
                ShareApplication = job.ShareApplication ? 1 : 0,
                SourceType = (int)job.DownloadSourceType,
                IsEnabled = job.Enabled ? 1 : 0,
                ExecuteCommandType = job.ExecuteCommandType.ToString(),
                ExecutePreCommand = job.ExecutePreCommand,
                ExecutePreCommandType = job.ExecutePreCommandType.ToString(),
                SourceTemplate = job.SourceTemplate,
                Category = job.Category,
                ExclusiveDownload = job.ExclusiveDownload ? 1 : 0,
                CheckForUpdateOnly = job.CheckForUpdatesOnly ? 1 : 0,
                UserNotes = job.UserNotes,
                WebsiteUrl = job.WebsiteUrl,
                UserAgent = job.UserAgent,
                PreviousRelativeLocation = GetPreviousRelativeLocation(job),
                HashType = (int)job.HashType,
                HashVariable = job.HashVariable,
                NumberOfRevisions = job.NumberOfRevisions,
                LastFileSize = job.LastFileSize,
                LastFileDate = job.LastFileDate,
                IgnoreFileInformation = job.IgnoreFileInformation ? 1 : 0
            };

            return jsonJob;
        }

        /// <summary>
        /// Converts a JSON snippet to a Snippet
        /// </summary>
        private static Snippet ConvertFromJson(JsonSnippet jsonSnippet)
        {
            Snippet snippet = new Snippet
            {
                Name = jsonSnippet.Name,
                Text = jsonSnippet.Text,
                Type = (Ketarin.ScriptType)Enum.Parse(typeof(Ketarin.ScriptType), jsonSnippet.Type ?? string.Empty)
            };

            // Set the GUID
            snippet.Guid = string.IsNullOrEmpty(jsonSnippet.SnippetGuid) ? Guid.Empty : Guid.Parse(jsonSnippet.SnippetGuid);

            return snippet;
        }

        /// <summary>
        /// Converts a Snippet to a JSON snippet
        /// </summary>
        private static JsonSnippet ConvertToJson(Snippet snippet)
        {
            JsonSnippet jsonSnippet = new JsonSnippet
            {
                SnippetGuid = snippet.Guid.ToString(),
                Name = snippet.Name,
                Text = snippet.Text,
                Type = snippet.Type.ToString()
            };

            return jsonSnippet;
        }
    }
}
