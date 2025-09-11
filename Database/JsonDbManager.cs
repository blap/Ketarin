using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Ketarin.Database
{
    /// <summary>
    /// JSON representation of an ApplicationJob
    /// </summary>
    public class JsonApplicationJob
    {
        public string? JobGuid { get; set; }
        public string? ApplicationName { get; set; }
        public string? TargetPath { get; set; }
        public string? FixedDownloadUrl { get; set; }
        public string? FileHippoId { get; set; }
        public string? FileHippoVersion { get; set; }
        public string? GitHubRepositoryId { get; set; }
        public string? GitHubVersion { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? DownloadDate { get; set; }
        public string? PreviousLocation { get; set; }
        public string? ExecuteCommand { get; set; }
        public string? HttpReferer { get; set; }
        public string? VariableChangeIndicator { get; set; }
        public string? VariableChangeIndicatorLastContent { get; set; }
        public int DeletePreviousFile { get; set; }
        public int DownloadBeta { get; set; }
        public int CanBeShared { get; set; }
        public int ShareApplication { get; set; }
        public int SourceType { get; set; }
        public int IsEnabled { get; set; }
        public string? ExecuteCommandType { get; set; }
        public string? ExecutePreCommand { get; set; }
        public string? ExecutePreCommandType { get; set; }
        public string? SourceTemplate { get; set; }
        public string? Category { get; set; }
        public int ExclusiveDownload { get; set; }
        public int CheckForUpdateOnly { get; set; }
        public string? UserNotes { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? UserAgent { get; set; }
        public string? PreviousRelativeLocation { get; set; }
        public int HashType { get; set; }
        public string? HashVariable { get; set; }
        public int NumberOfRevisions { get; set; }
        public long LastFileSize { get; set; }
        public DateTime? LastFileDate { get; set; }
        public int IgnoreFileInformation { get; set; }
    }
    
    /// <summary>
    /// JSON representation of a Variable
    /// </summary>
    public class JsonVariable
    {
        public string? JobGuid { get; set; }
        public string? VariableName { get; set; }
        public int VariableType { get; set; }
        public string? Url { get; set; }
        public string? StartText { get; set; }
        public string? EndText { get; set; }
        public string? RegularExpression { get; set; }
        public string? TextualContent { get; set; }
        public string? CachedContent { get; set; }
        public int RegexRightToLeft { get; set; }
        public string? PostData { get; set; }
    }
    
    /// <summary>
    /// JSON representation of a SetupInstruction
    /// </summary>
    public class JsonSetupInstruction
    {
        public string? JobGuid { get; set; }
        public int Position { get; set; }
        public string? Data { get; set; }
    }
    
    /// <summary>
    /// JSON representation of a Snippet
    /// </summary>
    public class JsonSnippet
    {
        public string? SnippetGuid { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Text { get; set; }
    }

    /// <summary>
    /// A JSON-based database manager to replace SQLite
    /// </summary>
    public class JsonDbManager
    {
        private static string? m_DatabasePath;
        private static readonly object lockObject = new object();
        
        // In-memory data structures to replace database tables
        private static Dictionary<string, object> settings = new Dictionary<string, object>();
        private static List<JsonApplicationJob> jobs = new List<JsonApplicationJob>();
        private static List<JsonVariable> variables = new List<JsonVariable>();
        private static List<JsonSetupInstruction> setupInstructions = new List<JsonSetupInstruction>();
        private static List<JsonSnippet> snippets = new List<JsonSnippet>();
        
        /// <summary>
        /// Sets a predefined database path if necessary.
        /// </summary>
        public static string? DatabasePath
        {
            set { m_DatabasePath = (value == null) ? null : Path.GetFullPath(value); }
            get { return m_DatabasePath; }
        }
        
        /// <summary>
        /// Builds a new proxy object from the settings.
        /// Returns null if no valid proxy exists.
        /// </summary>
        public static WebProxy? Proxy
        {
            get
            {
                if (settings.TryGetValue("ProxyServer", out object? serverObj) && 
                    settings.TryGetValue("ProxyPort", out object? portObj))
                {
                    string? server = serverObj?.ToString();
                    if (server == null) return null;
                    
                    int port;
                    try
                    {
                        port = Convert.ToInt32(portObj?.ToString() ?? "0");
                    }
                    catch
                    {
                        return null;
                    }

                    if (string.IsNullOrEmpty(server) || port <= 0) return null;

                    string? username = settings.TryGetValue("ProxyUser", out object? userObj) ? userObj?.ToString() : null;
                    string? password = settings.TryGetValue("ProxyPassword", out object? passObj) ? passObj?.ToString() : null;

                    string address = "http://" + server + ":" + port;

                    try
                    {
                        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                        {
                            return new WebProxy(address, true);
                        }
                        else
                        {
                            NetworkCredential credentials = new NetworkCredential(username ?? string.Empty, password ?? string.Empty);
                            return new WebProxy(address, true, null, credentials);
                        }
                    }
                    catch (UriFormatException)
                    {
                        return null;
                    }
                }
                return null;
            }
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
        /// Initializes the database by loading data from JSON files
        /// </summary>
        public static void Initialize()
        {
            lock (lockObject)
            {
                if (m_DatabasePath == null)
                {
                    // Only determine the path once
                    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ketarin", "jobs.json");
                    // Is a special path set in the registry?
                    if (settings.TryGetValue("Ketarin/DatabasePath", out object? regPathObj))
                    {
                        string? regPath = regPathObj?.ToString();
                        if (regPath != null && !string.IsNullOrEmpty(regPath) && File.Exists(regPath))
                        {
                            path = regPath;
                        }
                    }
                    // Or is there a database file in the startup directory?
                    string localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "jobs.json");
                    if (File.Exists(localPath))
                    {
                        path = localPath;
                    }

                    string? directoryName = Path.GetDirectoryName(path);
                    if (directoryName != null)
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    m_DatabasePath = path;
                }
                
                // Load data from JSON files
                LoadData();
            }
        }
        
        /// <summary>
        /// Loads data from JSON files
        /// </summary>
        private static void LoadData()
        {
            try
            {
                string? directoryName = Path.GetDirectoryName(m_DatabasePath);
                string settingsPath = Path.Combine(directoryName ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "settings.json");
                if (File.Exists(settingsPath))
                {
                    string json = File.ReadAllText(settingsPath);
                    settings = JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
                }
                
                string jobsPath = Path.Combine(directoryName ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "jobs.json");
                if (File.Exists(jobsPath))
                {
                    string json = File.ReadAllText(jobsPath);
                    jobs = JsonSerializer.Deserialize<List<JsonApplicationJob>>(json) ?? new List<JsonApplicationJob>();
                }
                
                string variablesPath = Path.Combine(directoryName ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "variables.json");
                if (File.Exists(variablesPath))
                {
                    string json = File.ReadAllText(variablesPath);
                    variables = JsonSerializer.Deserialize<List<JsonVariable>>(json) ?? new List<JsonVariable>();
                }
                
                string setupPath = Path.Combine(directoryName ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "setupinstructions.json");
                if (File.Exists(setupPath))
                {
                    string json = File.ReadAllText(setupPath);
                    setupInstructions = JsonSerializer.Deserialize<List<JsonSetupInstruction>>(json) ?? new List<JsonSetupInstruction>();
                }
                
                string snippetsPath = Path.Combine(directoryName ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "snippets.json");
                if (File.Exists(snippetsPath))
                {
                    string json = File.ReadAllText(snippetsPath);
                    snippets = JsonSerializer.Deserialize<List<JsonSnippet>>(json) ?? new List<JsonSnippet>();
                }
            }
            catch (Exception ex)
            {
                // Log error but continue with empty data
                System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Saves all data to JSON files
        /// </summary>
        public static void SaveData()
        {
            lock (lockObject)
            {
                try
                {
                    string? saveDirectoryName = Path.GetDirectoryName(m_DatabasePath);
                    string settingsPath = Path.Combine(saveDirectoryName ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "settings.json");
                    string settingsJson = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(settingsPath, settingsJson);
                    
                    string jobsPath = Path.Combine(saveDirectoryName ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "jobs.json");
                    string jobsJson = JsonSerializer.Serialize(jobs, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(jobsPath, jobsJson);
                    
                    string variablesPath = Path.Combine(saveDirectoryName ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "variables.json");
                    string variablesJson = JsonSerializer.Serialize(variables, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(variablesPath, variablesJson);
                    
                    string setupPath = Path.Combine(saveDirectoryName ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "setupinstructions.json");
                    string setupJson = JsonSerializer.Serialize(setupInstructions, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(setupPath, setupJson);
                    
                    string snippetsPath = Path.Combine(saveDirectoryName ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "snippets.json");
                    string snippetsJson = JsonSerializer.Serialize(snippets, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(snippetsPath, snippetsJson);
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Debug.WriteLine($"Error saving data: {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// Gets or sets a setting value
        /// </summary>
        public static object? GetSetting(string path)
        {
            lock (lockObject)
            {
                settings.TryGetValue(path, out object? value);
                return value ?? null;
            }
        }
        
        /// <summary>
        /// Sets a setting value
        /// </summary>
        public static void SetSetting(string path, object value)
        {
            lock (lockObject)
            {
                settings[path] = value;
                SaveData();
            }
        }
        
        /// <summary>
        /// Gets all application jobs
        /// </summary>
        public static JsonApplicationJob[] GetJobs()
        {
            lock (lockObject)
            {
                return jobs.ToArray();
            }
        }
        
        /// <summary>
        /// Adds or updates an application job
        /// </summary>
        public static void SaveJob(JsonApplicationJob job)
        {
            lock (lockObject)
            {
                // Remove existing job with same GUID if exists
                jobs.RemoveAll(j => j.JobGuid == job.JobGuid);
                // Add the new job
                jobs.Add(job);
                SaveData();
            }
        }
        
        /// <summary>
        /// Deletes an application job
        /// </summary>
        public static void DeleteJob(string jobGuid)
        {
            lock (lockObject)
            {
                jobs.RemoveAll(j => j.JobGuid == jobGuid);
                // Also remove associated variables and setup instructions
                variables.RemoveAll(v => v.JobGuid == jobGuid);
                setupInstructions.RemoveAll(s => s.JobGuid == jobGuid);
                SaveData();
            }
        }
        
        /// <summary>
        /// Gets variables for a specific job
        /// </summary>
        public static JsonVariable[] GetVariables(string jobGuid)
        {
            lock (lockObject)
            {
                return variables.FindAll(v => v.JobGuid == jobGuid).ToArray();
            }
        }
        
        /// <summary>
        /// Saves a variable
        /// </summary>
        public static void SaveVariable(JsonVariable variable)
        {
            lock (lockObject)
            {
                // Remove existing variable if exists
                variables.RemoveAll(v => v.JobGuid == variable.JobGuid && v.VariableName == variable.VariableName);
                // Add the new variable
                variables.Add(variable);
                SaveData();
            }
        }
        
        /// <summary>
        /// Deletes a variable
        /// </summary>
        public static void DeleteVariable(string jobGuid, string variableName)
        {
            lock (lockObject)
            {
                variables.RemoveAll(v => v.JobGuid == jobGuid && v.VariableName == variableName);
                SaveData();
            }
        }
        
        /// <summary>
        /// Gets setup instructions for a specific job
        /// </summary>
        public static JsonSetupInstruction[] GetSetupInstructions(string jobGuid)
        {
            lock (lockObject)
            {
                return setupInstructions.FindAll(s => s.JobGuid == jobGuid).ToArray();
            }
        }
        
        /// <summary>
        /// Saves a setup instruction
        /// </summary>
        public static void SaveSetupInstruction(JsonSetupInstruction instruction)
        {
            lock (lockObject)
            {
                // Remove existing instruction if exists
                setupInstructions.RemoveAll(s => s.JobGuid == instruction.JobGuid && s.Position == instruction.Position);
                // Add the new instruction
                setupInstructions.Add(instruction);
                SaveData();
            }
        }
        
        /// <summary>
        /// Deletes setup instructions for a job
        /// </summary>
        public static void DeleteSetupInstructions(string jobGuid)
        {
            lock (lockObject)
            {
                setupInstructions.RemoveAll(s => s.JobGuid == jobGuid);
                SaveData();
            }
        }
        
        /// <summary>
        /// Gets all snippets
        /// </summary>
        public static JsonSnippet[] GetSnippets()
        {
            lock (lockObject)
            {
                return snippets.ToArray();
            }
        }
        
        /// <summary>
        /// Saves a snippet
        /// </summary>
        public static void SaveSnippet(JsonSnippet snippet)
        {
            lock (lockObject)
            {
                // Remove existing snippet if exists
                snippets.RemoveAll(s => s.SnippetGuid == snippet.SnippetGuid);
                // Add the new snippet
                snippets.Add(snippet);
                SaveData();
            }
        }
        
        /// <summary>
        /// Deletes a snippet
        /// </summary>
        public static void DeleteSnippet(string snippetGuid)
        {
            lock (lockObject)
            {
                snippets.RemoveAll(s => s.SnippetGuid == snippetGuid);
                SaveData();
            }
        }
        
        /// <summary>
        /// Checks if an application job with the specified GUID exists
        /// </summary>
        public static bool ApplicationExists(string jobGuid)
        {
            lock (lockObject)
            {
                return jobs.Exists(j => j.JobGuid == jobGuid);
            }
        }
        
        /// <summary>
        /// Loads global variables
        /// </summary>
        public static Dictionary<string, string> LoadGlobalVariables()
        {
            lock (lockObject)
            {
                // For now, return an empty dictionary
                // In a real implementation, you would load from a JSON file
                string directoryPath = Path.GetDirectoryName(m_DatabasePath) ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string globalVariablesPath = Path.Combine(directoryPath, "globalvariables.json");
                if (File.Exists(globalVariablesPath))
                {
                    try
                    {
                        string json = File.ReadAllText(globalVariablesPath);
                        return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue with empty data
                        System.Diagnostics.Debug.WriteLine($"Error loading global variables: {ex.Message}");
                    }
                }
                return new Dictionary<string, string>();
            }
        }
        
        /// <summary>
        /// Saves global variables
        /// </summary>
        public static void SaveGlobalVariables(Dictionary<string, string> globalVariables)
        {
            lock (lockObject)
            {
                try
                {
                    string directoryPath = Path.GetDirectoryName(m_DatabasePath) ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    string globalVariablesPath = Path.Combine(directoryPath, "globalvariables.json");
                    string json = JsonSerializer.Serialize(globalVariables, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(globalVariablesPath, json);
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Debug.WriteLine($"Error saving global variables: {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// Gets settings from the database
        /// </summary>
        public static Dictionary<string, string> GetSettings()
        {
            lock (lockObject)
            {
                // Convert object dictionary to string dictionary
                Dictionary<string, string> stringSettings = new Dictionary<string, string>();
                foreach (var kvp in settings)
                {
                    stringSettings[kvp.Key] = kvp.Value?.ToString() ?? string.Empty;
                }
                return stringSettings;
            }
        }
        
        /// <summary>
        /// Sets settings in the database
        /// </summary>
        public static void SetSettings(Dictionary<string, string> newSettings)
        {
            lock (lockObject)
            {
                // Clear existing settings and add new ones
                settings.Clear();
                foreach (var kvp in newSettings)
                {
                    settings[kvp.Key] = kvp.Value;
                }
                SaveData();
            }
        }
        
        /// <summary>
        /// Gets all setup lists from the database
        /// </summary>
        public static ApplicationList[] GetSetupLists()
        {
            lock (lockObject)
            {
                // For now, return an empty array since we don't have this functionality
                return new ApplicationList[0];
            }
        }
        
    }
}