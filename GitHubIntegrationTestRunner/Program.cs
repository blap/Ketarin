using System;
using Ketarin;

namespace GitHubIntegrationTestRunner
{
    class Program
    {
        // Renamed from Main to RunTests to avoid entry point conflict
        static void RunTests(string[] args)
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
                
                Console.WriteLine("All tests completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed with exception: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        
        static void TestGetGitHubIdFromUrl()
        {
            Console.WriteLine("Testing GetGitHubIdFromUrl...");
            
            // Test valid GitHub URL
            string url1 = "https://github.com/owner/repo";
            string result1 = GitHubServices.GetGitHubIdFromUrl(url1);
            Console.WriteLine($"Input: {url1}, Output: {result1}");
            if (result1 != "owner/repo")
                throw new Exception($"Expected 'owner/repo', but got '{result1}'");
            
            // Test valid GitHub URL with releases
            string url2 = "https://github.com/owner/repo/releases";
            string result2 = GitHubServices.GetGitHubIdFromUrl(url2);
            Console.WriteLine($"Input: {url2}, Output: {result2}");
            if (result2 != "owner/repo")
                throw new Exception($"Expected 'owner/repo', but got '{result2}'");
            
            // Test invalid URL
            string url3 = "https://example.com/owner/repo";
            string result3 = GitHubServices.GetGitHubIdFromUrl(url3);
            Console.WriteLine($"Input: {url3}, Output: {result3}");
            if (result3 != url3)
                throw new Exception($"Expected '{url3}', but got '{result3}'");
            
            Console.WriteLine("GetGitHubIdFromUrl tests passed.\n");
        }
        
        static void TestSourceType()
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
        
        static void TestApplicationJobProperties()
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
    }
}