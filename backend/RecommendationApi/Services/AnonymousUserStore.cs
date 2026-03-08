using System.Collections.Concurrent;

namespace RecommendationApi.Services;

/// <summary>
/// Tracks daily session creation per anonymous user.
/// Key design: "{anonymousId}:{yyyy-MM-dd}" → session count.
/// Old date keys naturally become stale (never matched again), and are
/// periodically cleaned up to prevent unbounded memory growth.
/// </summary>
public class AnonymousUserStore
{
    private const int DailySessionLimit = 1;
    private const int MaxKeys = 200_000;

    // key: "{anonId}:{date}", value: sessions created on that date
    private readonly ConcurrentDictionary<string, int> _usage = new();

    private DateTime _lastCleanup = DateTime.UtcNow;

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>
    /// Atomically consumes one daily quota unit for the given anonymous user.
    /// Returns false if limit is exceeded or store is saturated.
    /// </summary>
    public bool TryConsumeDailyQuota(string anonymousId)
    {
        if (_usage.Count >= MaxKeys)
        {
            TryCleanup(force: true);
            if (_usage.Count >= MaxKeys)
            {
                return false;
            }
        }

        var key = DayKey(anonymousId);
        var updated = _usage.AddOrUpdate(key, 1, (_, existing) => existing + 1);
        TryCleanup();
        return updated <= DailySessionLimit;
    }

    // ── Private ───────────────────────────────────────────────────────────────

    private static string DayKey(string anonymousId)
        => $"{anonymousId}:{DateTime.UtcNow:yyyy-MM-dd}";

    /// <summary>
    /// Removes stale keys (from previous days) once per hour.
    /// Prevents unbounded dictionary growth without needing a background timer.
    /// </summary>
    private void TryCleanup(bool force = false)
    {
        if (!force && (DateTime.UtcNow - _lastCleanup).TotalHours < 1) return;
        _lastCleanup = DateTime.UtcNow;

        var todaySuffix = $":{DateTime.UtcNow:yyyy-MM-dd}";
        foreach (var key in _usage.Keys)
        {
            if (!key.EndsWith(todaySuffix, StringComparison.Ordinal))
                _usage.TryRemove(key, out _);
        }
    }
}
