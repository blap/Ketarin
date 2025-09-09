using System;
using System.IO;
using System.Text.Json;

namespace Ketarin.Database
{
    /// <summary>
    /// Manages application configuration using JSON instead of app.config
    /// </summary>
    public static class ConfigurationManager
    {
        private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        private static ConfigurationSettings? _settings;
        
        /// <summary>
        /// Gets the current configuration settings
        /// </summary>
        public static ConfigurationSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    LoadSettings();
                }
                return _settings;
            }
        }
        
        /// <summary>
        /// Loads settings from the JSON configuration file
        /// </summary>
        private static void LoadSettings()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    string json = File.ReadAllText(ConfigFilePath);
                    _settings = JsonSerializer.Deserialize<ConfigurationSettings>(json);
                }
                else
                {
                    // Create default settings if file doesn't exist
                    _settings = new ConfigurationSettings
                    {
                        Ketarin = new KetarinSettings
                        {
                            EnableWindowsFormsHighDpiAutoResizing = true
                        },
                        Runtime = new RuntimeSettings
                        {
                            GeneratePublisherEvidence = false
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                // Log error and use default settings
                System.Diagnostics.Debug.WriteLine($"Error loading configuration: {ex.Message}");
                
                _settings = new ConfigurationSettings
                {
                    Ketarin = new KetarinSettings
                    {
                        EnableWindowsFormsHighDpiAutoResizing = true
                    },
                    Runtime = new RuntimeSettings
                    {
                        GeneratePublisherEvidence = false
                    }
                };
            }
        }
        
        /// <summary>
        /// Saves the current settings to the JSON configuration file
        /// </summary>
        public static void SaveSettings()
        {
            try
            {
                string json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error saving configuration: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Gets a setting value by key
        /// </summary>
        public static string GetSetting(string key)
        {
            switch (key.ToLower())
            {
                case "enablewindowsformshighdpiautoresizing":
                    return Settings.Ketarin.EnableWindowsFormsHighDpiAutoResizing.ToString();
                case "generatepublisherevidence":
                    return Settings.Runtime.GeneratePublisherEvidence.ToString();
                default:
                    return null;
            }
        }
        
        /// <summary>
        /// Sets a setting value by key
        /// </summary>
        public static void SetSetting(string key, string value)
        {
            switch (key.ToLower())
            {
                case "enablewindowsformshighdpiautoresizing":
                    if (bool.TryParse(value, out bool enableHighDpi))
                    {
                        Settings.Ketarin.EnableWindowsFormsHighDpiAutoResizing = enableHighDpi;
                    }
                    break;
                case "generatepublisherevidence":
                    if (bool.TryParse(value, out bool generateEvidence))
                    {
                        Settings.Runtime.GeneratePublisherEvidence = generateEvidence;
                    }
                    break;
            }
            
            SaveSettings();
        }
    }
    
    /// <summary>
    /// Represents the overall configuration settings
    /// </summary>
    public class ConfigurationSettings
    {
        public KetarinSettings Ketarin { get; set; } = new KetarinSettings();
        public RuntimeSettings Runtime { get; set; } = new RuntimeSettings();
    }
    
    /// <summary>
    /// Represents Ketarin-specific settings
    /// </summary>
    public class KetarinSettings
    {
        public bool EnableWindowsFormsHighDpiAutoResizing { get; set; }
    }
    
    /// <summary>
    /// Represents runtime settings
    /// </summary>
    public class RuntimeSettings
    {
        public bool GeneratePublisherEvidence { get; set; }
    }
}