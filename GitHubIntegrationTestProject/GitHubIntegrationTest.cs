using System;
using Ketarin;

class GitHubIntegrationTest
{
    public static void RunTests()
    {
        Console.WriteLine("Testing GitHub integration...");
        
        // Test GetGitHubIdFromUrl
        TestGetGitHubIdFromUrl();
        
        // Test SourceType enum
        TestSourceType();
        
        // Test ApplicationJob properties
        TestApplicationJobProperties();
        
        Console.WriteLine("All tests completed.");
    }
    
    static void TestGetGitHubIdFromUrl()
    {
        Console.WriteLine("Testing GetGitHubIdFromUrl...");
        
        // Test valid GitHub URL
        string url1 = "https://github.com/owner/repo";
        string result1 = GitHubServices.GetGitHubIdFromUrl(url1);
        Console.WriteLine($"Input: {url1}, Output: {result1}");
        AssertEqual("owner/repo", result1);
        
        // Test valid GitHub URL with releases
        string url2 = "https://github.com/owner/repo/releases";
        string result2 = GitHubServices.GetGitHubIdFromUrl(url2);
        Console.WriteLine($"Input: {url2}, Output: {result2}");
        AssertEqual("owner/repo", result2);
        
        // Test invalid URL
        string url3 = "https://example.com/owner/repo";
        string result3 = GitHubServices.GetGitHubIdFromUrl(url3);
        Console.WriteLine($"Input: {url3}, Output: {result3}");
        AssertEqual(url3, result3);
        
        Console.WriteLine("GetGitHubIdFromUrl tests passed.\n");
    }
    
    static void TestSourceType()
    {
        Console.WriteLine("Testing SourceType enum...");
        
        AssertEqual(0, (int)SourceType.FixedUrl);
        AssertEqual(1, (int)SourceType.FileHippo);
        AssertEqual(2, (int)SourceType.GitHub);
        
        Console.WriteLine("SourceType enum tests passed.\n");
    }
    
    static void TestApplicationJobProperties()
    {
        Console.WriteLine("Testing ApplicationJob GitHub properties...");
        
        ApplicationJob job = new ApplicationJob();
        
        // Test initialization
        AssertNotNull(job.GitHubRepositoryId);
        AssertNotNull(job.GitHubVersion);
        AssertEqual("", job.GitHubRepositoryId);
        AssertEqual("", job.GitHubVersion);
        
        // Test setting values
        string repoId = "owner/repo";
        string version = "1.0.0";
        
        job.GitHubRepositoryId = repoId;
        job.GitHubVersion = version;
        
        AssertEqual(repoId, job.GitHubRepositoryId);
        AssertEqual(version, job.GitHubVersion);
        
        Console.WriteLine("ApplicationJob GitHub properties tests passed.\n");
    }
    
    static void AssertEqual(object expected, object actual)
    {
        if (!expected.Equals(actual))
        {
            throw new Exception($"Assertion failed: expected {expected}, but got {actual}");
        }
    }
    
    static void AssertNotNull(object obj)
    {
        if (obj == null)
        {
            throw new Exception("Assertion failed: object is null");
        }
    }
}