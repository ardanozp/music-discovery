using Microsoft.Extensions.Caching.Memory;
using RecommendationApi.Models;

namespace RecommendationApi.Services;

/// <summary>
/// In-memory store for user recommendation sessions.
/// Registered as a singleton — survives the lifetime of the application process.
/// </summary>
public class SessionStore
{
    private const int AlbumsPerRound  = 3;

    private readonly RecommendationService _recommender;
    private readonly IMemoryCache _cache;

    public SessionStore(RecommendationService recommender, IMemoryCache cache)
    {
        _recommender = recommender;
        _cache = cache;
    }

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>
    /// Creates a new session for the given preferences, picks the first 3 albums,
    /// and returns the initial response.
    /// </summary>
    public SessionResponse CreateSession(RecommendationRequest preferences)
    {
        var session = new SessionState
        {
            Id          = Guid.NewGuid(),
            Preferences = preferences
        };

        var albums = _recommender.GetRecommendations(preferences, AlbumsPerRound, excludeMbids: null);
        TrackShown(session, albums);

        var cacheOptions = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromHours(2),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8),
            Size = 1
        };
        _cache.Set(session.Id, session, cacheOptions);

        return ToResponse(session, albums);
    }

    /// <summary>
    /// Attempts a restart for the given session.
    /// Returns null if the session does not exist.
    /// Throws <see cref="InvalidOperationException"/> if no restarts remain.
    /// </summary>
    public SessionResponse? Restart(Guid sessionId)
    {
        if (!_cache.TryGetValue(sessionId, out SessionState? session) || session == null)
            return null;

        if (session.RestartsRemaining <= 0)
            throw new InvalidOperationException("No restarts remaining for this session.");

        session.RestartsRemaining--;

        var albums = _recommender.GetRecommendations(
            session.Preferences,
            AlbumsPerRound,
            excludeMbids: session.ShownMbids);

        TrackShown(session, albums);
        return ToResponse(session, albums);
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private static void TrackShown(SessionState session, IEnumerable<AlbumDto> albums)
    {
        foreach (var album in albums)
            session.ShownMbids.Add(album.Id);
    }

    private static SessionResponse ToResponse(SessionState session, List<AlbumDto> albums)
        => new()
        {
            SessionId         = session.Id,
            Albums            = albums,
            RestartsRemaining = session.RestartsRemaining
        };
}
