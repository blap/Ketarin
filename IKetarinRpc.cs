using System;

namespace Ketarin
{
    // Removed XML-RPC interface for .NET 9 compatibility
    // Online database functionality is disabled in this version

    public struct RpcAppGuidAndDate
    {
        public string ApplicationGuid;
        public int UpdatedAt;

        public RpcAppGuidAndDate(Guid guid, DateTime? date)
        {
            ApplicationGuid = guid.ToString();
            UpdatedAt = date.HasValue ? RpcApplication.DotNetToUnix(date.Value) : 0;
        }
    }

    public struct RpcApplication
    {
        public static DateTime UnixToDotNet(int unixTimestamp)
        {
            return new DateTime(1970, 1, 1).AddSeconds(unixTimestamp);
        }

        public static int DotNetToUnix(DateTime date)
        {
            return Convert.ToInt32((date - new DateTime(1970, 1, 1)).TotalSeconds);
        }

        public string ApplicationName;
        public int UpdatedAt;
        public int ShareId;
        public int UseCount;

        public DateTime UpdatedAtDate
        {
            get
            {
                return UnixToDotNet(UpdatedAt);
            }
        }
    }
}