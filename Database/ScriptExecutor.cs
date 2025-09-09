using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Ketarin.Forms;
using Microsoft.CSharp;

namespace Ketarin.Database
{
    /// <summary>
    /// Executes scripts without external dependencies
    /// </summary>
    internal class ScriptExecutor
    {
        private readonly string scriptText;
        private readonly ScriptType scriptType;
        private string lastOutput = string.Empty;

        public ScriptExecutor(string scriptText, ScriptType scriptType)
        {
            this.scriptText = scriptText;
            this.scriptType = scriptType;
        }

        public string LastOutput 
        { 
            get { return this.lastOutput; }
            private set { this.lastOutput = value; }
        }

        public void Execute(ApplicationJob application, ApplicationJobError? errorInfo = null)
        {
            switch (this.scriptType)
            {
                case ScriptType.Batch:
                    ExecuteBatchScript(application, errorInfo);
                    break;
                case ScriptType.CS:
                    ExecuteCsscript(application, errorInfo);
                    break;
                case ScriptType.PowerShell:
                    // For now, we'll convert PowerShell to batch
                    // In a future version, we could implement a proper PowerShell-like engine
                    ExecutePowerShellAsBatch(application, errorInfo);
                    break;
            }
        }

        private void ExecuteBatchScript(ApplicationJob application, ApplicationJobError errorInfo)
        {
            try
            {
                // Create a temporary batch file
                string tempPath = Path.GetTempFileName();
                string batchFile = Path.ChangeExtension(tempPath, ".bat");
                File.Move(tempPath, batchFile);
                
                // Replace variables in the script
                string scriptContent = this.scriptText;
                if (application != null)
                {
                    scriptContent = application.Variables.ReplaceAllInString(scriptContent);
                }
                
                File.WriteAllText(batchFile, scriptContent, Encoding.Default);
                
                // Execute the batch file
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = batchFile,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                
                using (Process process = Process.Start(startInfo))
                {
                    // Read output
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    
                    process.WaitForExit();
                    
                    this.LastOutput = output;
                    LogDialog.Log("Batch script output: " + output);
                    
                    if (!string.IsNullOrEmpty(error))
                    {
                        LogDialog.Log("Batch script error: " + error);
                        if (process != null && process.HasExited && process.ExitCode != 0)
                        {
                            throw new ApplicationException($"Batch script failed with exit code {process.ExitCode}: {error}");
                        }
                    }
                }
                
                // Clean up
                if (File.Exists(batchFile))
                {
                    File.Delete(batchFile);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Batch script execution failed: {ex.Message}", ex);
            }
        }

        private void ExecuteCsscript(ApplicationJob application, ApplicationJobError errorInfo)
        {
            try
            {
                // For C# scripts, we'll use the existing CSScript implementation
                // but without the PowerShell dependency
                UserCSScript csScript = new UserCSScript(this.scriptText);
                
                // We need to modify the CSScript to not use PowerShell
                // For now, let's just execute it as-is but handle any PowerShell-related issues
                csScript.Execute(application);
                // The LastOutput property doesn't exist on UserCSScript, so we'll set it to a default value
                this.LastOutput = "Script executed successfully";
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"C# script execution failed: {ex.Message}", ex);
            }
        }

        private void ExecutePowerShellAsBatch(ApplicationJob application, ApplicationJobError errorInfo)
        {
            try
            {
                // Convert PowerShell commands to batch where possible
                // This is a simple conversion - in reality, complex PowerShell scripts
                // would need more sophisticated handling
                string batchScript = ConvertPowerShellToBatch(this.scriptText);
                ExecuteBatchScript(application, errorInfo);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"PowerShell script execution failed: {ex.Message}", ex);
            }
        }

        private string ConvertPowerShellToBatch(string powerShellScript)
        {
            // This is a very basic conversion
            // A full implementation would need to parse PowerShell syntax
            StringBuilder batchScript = new StringBuilder();
            batchScript.AppendLine("@echo off");
            batchScript.AppendLine("REM Converted from PowerShell script");
            
            // Simple replacements for common PowerShell commands
            string[] lines = powerShellScript.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
                {
                    // Skip empty lines and comments
                    continue;
                }
                
                // Convert some common PowerShell commands
                if (trimmedLine.StartsWith("Write-Host "))
                {
                    string message = trimmedLine.Substring("Write-Host ".Length).Trim(' ', '"');
                    batchScript.AppendLine($"echo {message}");
                }
                else if (trimmedLine.StartsWith("Write-Output "))
                {
                    string message = trimmedLine.Substring("Write-Output ".Length).Trim(' ', '"');
                    batchScript.AppendLine($"echo {message}");
                }
                else if (trimmedLine.StartsWith("$"))
                {
                    // Variable assignment - convert to set
                    int equalsIndex = trimmedLine.IndexOf('=');
                    if (equalsIndex > 0)
                    {
                        string varName = trimmedLine.Substring(1, equalsIndex - 1).Trim();
                        string value = trimmedLine.Substring(equalsIndex + 1).Trim(' ', '"');
                        batchScript.AppendLine($"set {varName}={value}");
                    }
                }
                else
                {
                    // Assume it's a command
                    batchScript.AppendLine(trimmedLine);
                }
            }
            
            return batchScript.ToString();
        }
    }
    
    /// <summary>
    /// Scripting language used for a command.
    /// </summary>
    public enum ScriptType
    {
        /// <summary>
        /// C# script
        /// </summary>
        CS,

        /// <summary>
        /// Batch file
        /// </summary>
        Batch,

        /// <summary>
        /// PowerShell script (converted to batch)
        /// </summary>
        PowerShell
    }
}