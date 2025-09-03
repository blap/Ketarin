using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Ketarin
{
    /// <summary>
    /// Helper class for performance optimizations and caching
    /// </summary>
    public static class PerformanceHelper
    {
        private static readonly ConcurrentDictionary<string, CacheEntry> _cache = new();
        private static readonly SemaphoreSlim _cacheSemaphore = new(1, 1);

        /// <summary>
        /// Cache entry with expiration
        /// </summary>
        private class CacheEntry
        {
            public string Value { get; set; }
            public DateTime ExpiresAt { get; set; }

            public bool IsExpired => DateTime.UtcNow > ExpiresAt;
        }

        /// <summary>
        /// Gets or sets the default cache duration in minutes
        /// </summary>
        public static int DefaultCacheDurationMinutes { get; set; } = 5;

        /// <summary>
        /// Gets cached value or executes function to get fresh value
        /// </summary>
        public static async Task<string> GetCachedOrExecuteAsync(
            string cacheKey,
            Func<Task<string>> valueFactory,
            int? cacheDurationMinutes = null)
        {
            var duration = cacheDurationMinutes ?? DefaultCacheDurationMinutes;

            await _cacheSemaphore.WaitAsync();
            try
            {
                // Check if we have a valid cached entry
                if (_cache.TryGetValue(cacheKey, out var entry) && !entry.IsExpired)
                {
                    return entry.Value;
                }

                // Execute the function to get fresh value
                var value = await valueFactory();

                // Cache the result
                _cache[cacheKey] = new CacheEntry
                {
                    Value = value,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(duration)
                };

                return value;
            }
            finally
            {
                _cacheSemaphore.Release();
            }
        }

        /// <summary>
        /// Clears expired cache entries
        /// </summary>
        public static async Task CleanupExpiredCacheAsync()
        {
            await _cacheSemaphore.WaitAsync();
            try
            {
                var expiredKeys = new System.Collections.Generic.List<string>();

                foreach (var kvp in _cache)
                {
                    if (kvp.Value.IsExpired)
                    {
                        expiredKeys.Add(kvp.Key);
                    }
                }

                foreach (var key in expiredKeys)
                {
                    _cache.TryRemove(key, out _);
                }
            }
            finally
            {
                _cacheSemaphore.Release();
            }
        }

        /// <summary>
        /// Clears all cache entries
        /// </summary>
        public static async Task ClearCacheAsync()
        {
            await _cacheSemaphore.WaitAsync();
            try
            {
                _cache.Clear();
            }
            finally
            {
                _cacheSemaphore.Release();
            }
        }

        /// <summary>
        /// Gets cache statistics
        /// </summary>
        public static (int TotalEntries, int ExpiredEntries) GetCacheStats()
        {
            int total = 0;
            int expired = 0;

            foreach (var kvp in _cache)
            {
                total++;
                if (kvp.Value.IsExpired)
                {
                    expired++;
                }
            }

            return (total, expired);
        }

        /// <summary>
        /// Starts a background task to periodically clean up expired cache entries
        /// </summary>
        public static void StartCacheCleanupTimer(TimeSpan interval)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(interval);
                    await CleanupExpiredCacheAsync();
                }
            });
        }
    }
}