namespace Ketarin.Downloader
{
    /// <summary>
    /// Settings for the downloader.
    /// </summary>
    internal class Settings
    {
        public static double MinSegmentLeftToStartNewSegment { get; set; } = 30;

        public static long MinSegmentSize { get; set; } = 200000;

        public static string ProxyAddress { get; set; } = string.Empty;

        public static string ProxyUserName { get; set; } = string.Empty;

        public static string ProxyPassword { get; set; } = string.Empty;

        public static string ProxyDomain { get; set; } = string.Empty;

        public static bool UseProxy { get; set; }

        public static bool ProxyByPassOnLocal { get; set; }

        public static int ProxyPort { get; set; }
        
        // Added missing properties to fix compilation errors
        public static int MaxSegments { get; set; } = 1;
        
        public static int MaxRetries { get; set; } = 3;
        
        public static int RetryDelay { get; set; } = 5;
    }
}