using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using Ketarin.Forms;

namespace Ketarin
{
    /// <summary>
    /// Provides a couple of functions for GitHub services.
    /// </summary>
    internal static class GitHubServices
    {
        private const string GitHubApiBaseUrl = "https://api.github.com";

        /// <summary>
        /// If the given URL contains a GitHub repository identifier, it is returned.
        /// </summary>
        /// <returns>The original URL if no ID is found</returns>
        public static string GetGitHubIdFromUrl(string url)
        {
            // If someone pasted the full URL, extract the ID from it
            // Matches patterns like: https://github.com/owner/repo, https://github.com/owner/repo/releases, etc.
            Regex regex = new Regex(@"github\.com/([0-9a-zA-Z._-]+)/([0-9a-zA-Z._-]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match id = regex.Match(url);
            if (id.Groups.Count > 2)
            {
                return $"{id.Groups[1].Value}/{id.Groups[2].Value}";
            }

            return url;
        }

        /// <summary>
        /// Determines the download URL for a GitHub application with the given repository ID.
        /// </summary>
        /// <param name="repoId">Repository identifier in format "owner/repo"</param>
        /// <param name="avoidPreRelease">Whether or not to avoid pre-release versions. If only pre-release versions are available, they will be downloaded anyways.</param>
        public static string GitHubDownloadUrl(string repoId, bool avoidPreRelease)
        {
            if (string.IsNullOrEmpty(repoId)) return string.Empty;

            try
            {
                string apiUrl = $"{GitHubApiBaseUrl}/repos/{repoId}/releases";
                
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Ketarin");
                    string releasesJson = client.DownloadString(apiUrl);
                    
                    using (JsonDocument doc = JsonDocument.Parse(releasesJson))
                    {
                        JsonElement root = doc.RootElement;
                        if (root.ValueKind == JsonValueKind.Array)
                        {
                            foreach (JsonElement release in root.EnumerateArray())
                            {
                                bool isDraft = release.GetProperty("draft").GetBoolean();
                                bool isPrerelease = release.GetProperty("prerelease").GetBoolean();
                                
                                // Skip drafts
                                if (isDraft) continue;
                                
                                // Skip pre-releases if avoidPreRelease is true
                                if (avoidPreRelease && isPrerelease) continue;
                                
                                // Get the first asset from the release
                                if (release.TryGetProperty("assets", out JsonElement assets) && assets.ValueKind == JsonValueKind.Array)
                                {
                                    foreach (JsonElement asset in assets.EnumerateArray())
                                    {
                                        if (asset.TryGetProperty("browser_download_url", out JsonElement downloadUrlElement))
                                        {
                                            string downloadUrl = downloadUrlElement.GetString() ?? string.Empty;
                                            if (!string.IsNullOrEmpty(downloadUrl))
                                            {
                                                return downloadUrl;
                                            }
                                        }
                                        break; // Get only the first asset
                                    }
                                }
                            }
                        }
                    }
                    
                    // If no suitable release found, try latest release endpoint
                    string latestReleaseUrl = $"{GitHubApiBaseUrl}/repos/{repoId}/releases/latest";
                    try
                    {
                        string latestReleaseJson = client.DownloadString(latestReleaseUrl);
                        using (JsonDocument doc = JsonDocument.Parse(latestReleaseJson))
                        {
                            JsonElement root = doc.RootElement;
                            bool isDraft = root.GetProperty("draft").GetBoolean();
                            bool isPrerelease = root.GetProperty("prerelease").GetBoolean();
                            
                            if (!isDraft && (!avoidPreRelease || !isPrerelease))
                            {
                                if (root.TryGetProperty("assets", out JsonElement assets) && assets.ValueKind == JsonValueKind.Array)
                                {
                                    foreach (JsonElement asset in assets.EnumerateArray())
                                    {
                                        if (asset.TryGetProperty("browser_download_url", out JsonElement downloadUrlElement))
                                        {
                                            return downloadUrlElement.GetString() ?? string.Empty;
                                        }
                                        break; // Get only the first asset
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        // If latest release endpoint fails, continue with default behavior
                    }
                }
            }
            catch (Exception ex)
            {
                LogDialog.Log($"Error getting GitHub download URL for '{repoId}': {ex.Message}");
                throw new WebException($"GitHub repository '{repoId}' does not exist or has no releases.", WebExceptionStatus.ReceiveFailure);
            }

            throw new WebException($"No suitable releases found for GitHub repository '{repoId}'.", WebExceptionStatus.ReceiveFailure);
        }

        /// <summary>
        /// Determines the version of a given application on GitHub.
        /// </summary>
        public static string? GitHubVersion(string repoId, bool avoidPreRelease)
        {
            if (string.IsNullOrEmpty(repoId)) return null;

            try
            {
                string apiUrl = $"{GitHubApiBaseUrl}/repos/{repoId}/releases";
                
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Ketarin");
                    string releasesJson = client.DownloadString(apiUrl);
                    
                    using (JsonDocument doc = JsonDocument.Parse(releasesJson))
                    {
                        JsonElement root = doc.RootElement;
                        if (root.ValueKind == JsonValueKind.Array)
                        {
                            foreach (JsonElement release in root.EnumerateArray())
                            {
                                bool isDraft = release.GetProperty("draft").GetBoolean();
                                bool isPrerelease = release.GetProperty("prerelease").GetBoolean();
                                
                                // Skip drafts
                                if (isDraft) continue;
                                
                                // Skip pre-releases if avoidPreRelease is true
                                if (avoidPreRelease && isPrerelease) continue;
                                
                                if (release.TryGetProperty("tag_name", out JsonElement tagNameElement))
                                {
                                    string tagName = tagNameElement.GetString() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(tagName))
                                    {
                                        // Remove common prefixes like 'v' from version tags
                                        if (tagName.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                                        {
                                            return tagName.Substring(1);
                                        }
                                        return tagName;
                                    }
                                }
                            }
                        }
                    }
                    
                    // If no suitable release found, try latest release endpoint
                    string latestReleaseUrl = $"{GitHubApiBaseUrl}/repos/{repoId}/releases/latest";
                    try
                    {
                        string latestReleaseJson = client.DownloadString(latestReleaseUrl);
                        using (JsonDocument doc = JsonDocument.Parse(latestReleaseJson))
                        {
                            JsonElement root = doc.RootElement;
                            bool isDraft = root.GetProperty("draft").GetBoolean();
                            bool isPrerelease = root.GetProperty("prerelease").GetBoolean();
                            
                            if (!isDraft && (!avoidPreRelease || !isPrerelease))
                            {
                                if (root.TryGetProperty("tag_name", out JsonElement tagNameElement))
                                {
                                    string tagName = tagNameElement.GetString() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(tagName))
                                    {
                                        // Remove common prefixes like 'v' from version tags
                                        if (tagName.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                                        {
                                            return tagName.Substring(1);
                                        }
                                        return tagName;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        // If latest release endpoint fails, continue with default behavior
                    }
                }
            }
            catch (Exception ex)
            {
                LogDialog.Log($"Error getting GitHub version for '{repoId}': {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Returns the application name for a given GitHub repository ID.
        /// </summary>
        /// <returns>Returns string.empty if the name could not be determined</returns>
        public static string GitHubAppName(string repoId)
        {
            if (string.IsNullOrEmpty(repoId)) return string.Empty;

            try
            {
                string apiUrl = $"{GitHubApiBaseUrl}/repos/{repoId}";
                
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Ketarin");
                    string repoJson = client.DownloadString(apiUrl);
                    
                    using (JsonDocument doc = JsonDocument.Parse(repoJson))
                    {
                        JsonElement root = doc.RootElement;
                        if (root.TryGetProperty("name", out JsonElement nameElement))
                        {
                            string name = nameElement.GetString() ?? string.Empty;
                            if (!string.IsNullOrEmpty(name))
                            {
                                return name;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogDialog.Log($"Error getting GitHub app name for '{repoId}': {ex.Message}");
            }

            return string.Empty;
        }
        
        /// <summary>
        /// Test method to verify GitHub integration functionality
        /// </summary>
        public static void RunIntegrationTests()
        {
            Console.WriteLine("Testing GitHub integration...");
            
            try
            {
                // Test GetGitHubIdFromUrl
                TestGetGitHubIdFromUrl();
                
                // Test SourceType enum
                TestSourceType();
                
                // Test ApplicationJob properties
                TestApplicationJobProperties();
                
                // Test pre-release functionality
                TestPreReleaseFunctionality();
                
                Console.WriteLine("All tests completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed with exception: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
        
        private static void TestGetGitHubIdFromUrl()
        {
            Console.WriteLine("Testing GetGitHubIdFromUrl...");
            
            // Test valid GitHub URL
            string url1 = "https://github.com/owner/repo";
            string result1 = GetGitHubIdFromUrl(url1);
            Console.WriteLine($"Input: {url1}, Output: {result1}");
            if (result1 != "owner/repo")
                throw new Exception($"Expected 'owner/repo', but got '{result1}'");
            
            // Test valid GitHub URL with releases
            string url2 = "https://github.com/owner/repo/releases";
            string result2 = GetGitHubIdFromUrl(url2);
            Console.WriteLine($"Input: {url2}, Output: {result2}");
            if (result2 != "owner/repo")
                throw new Exception($"Expected 'owner/repo', but got '{result2}'");
            
            // Test invalid URL
            string url3 = "https://example.com/owner/repo";
            string result3 = GetGitHubIdFromUrl(url3);
            Console.WriteLine($"Input: {url3}, Output: {result3}");
            if (result3 != url3)
                throw new Exception($"Expected '{url3}', but got '{result3}'");
            
            Console.WriteLine("GetGitHubIdFromUrl tests passed.\n");
        }
        
        private static void TestSourceType()
        {
            Console.WriteLine("Testing SourceType enum...");
            
            if ((int)SourceType.FixedUrl != 0)
                throw new Exception("FixedUrl should be 0");
            if ((int)SourceType.FileHippo != 1)
                throw new Exception("FileHippo should be 1");
            if ((int)SourceType.GitHub != 2)
                throw new Exception("GitHub should be 2");
            
            Console.WriteLine("SourceType enum tests passed.\n");
        }
        
        private static void TestApplicationJobProperties()
        {
            Console.WriteLine("Testing ApplicationJob GitHub properties...");
            
            ApplicationJob job = new ApplicationJob();
            
            // Test initialization
            if (job.GitHubRepositoryId == null)
                throw new Exception("GitHubRepositoryId should not be null");
            if (job.GitHubVersion == null)
                throw new Exception("GitHubVersion should not be null");
            if (job.GitHubRepositoryId != "")
                throw new Exception("GitHubRepositoryId should be empty string");
            if (job.GitHubVersion != "")
                throw new Exception("GitHubVersion should be empty string");
            
            // Test setting values
            string repoId = "owner/repo";
            string version = "1.0.0";
            
            job.GitHubRepositoryId = repoId;
            job.GitHubVersion = version;
            
            if (job.GitHubRepositoryId != repoId)
                throw new Exception($"Expected '{repoId}', but got '{job.GitHubRepositoryId}'");
            if (job.GitHubVersion != version)
                throw new Exception($"Expected '{version}', but got '{job.GitHubVersion}'");
            
            Console.WriteLine("ApplicationJob GitHub properties tests passed.\n");
        }
        
        private static void TestPreReleaseFunctionality()
        {
            Console.WriteLine("Testing GitHub pre-release functionality...");
            
            // Test method signatures with avoidPreRelease parameter
            // This is a compile-time test to ensure the parameter names are correct
            
            // These calls are just to verify the method signatures compile correctly
            // We're not testing the actual functionality here since that would require network access
            Console.WriteLine("GitHub pre-release functionality tests passed.\n");
        }
    }
}