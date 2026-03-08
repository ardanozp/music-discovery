namespace RecommendationApi.Services;

/// <summary>
/// Converts string artist attributes from albums.json into 0.0–1.0 numeric scores.
/// All methods are pure and stateless.
/// </summary>
public static class ScoringService
{
    // ── Dimension weights (must sum to 1.0) ───────────────────────────────────
    public const float WeightEnergy      = 0.40f;
    public const float WeightFamiliarity = 0.20f;
    public const float WeightTime        = 0.40f;

    /// <summary>
    /// energy_level → softened score: high → 0.85, mid → 0.5, low/other → 0.15.
    /// Avoiding extremes (0 and 1) keeps energy from dominating when user is near center.
    /// </summary>
    public static float EnergyScore(string energyLevel) => energyLevel.ToLowerInvariant() switch
    {
        "high" => 0.85f,
        "mid"  => 0.50f,
        _      => 0.15f
    };

    /// <summary>
    /// Listeners count → familiarity score using log scale.
    /// ~50k listeners → 0.0, ~10M listeners → 1.0.
    /// Clamped to 0–1.
    /// </summary>
    public static float FamiliarityScore(long listeners)
    {
        const double logMin = 10.82;  // log(50_000)
        const double logMax = 16.12;  // log(10_000_000)
        if (listeners <= 0) return 0f;
        return Math.Clamp((float)((Math.Log(listeners) - logMin) / (logMax - logMin)), 0f, 1f);
    }

    /// <summary>
    /// Album release year → time score: 1960 → 0.0, 2026 → 1.0.
    /// Clamped so years outside the range don't exceed 0–1.
    /// </summary>
    public static float TimeScore(int year)
    {
        const int minYear = 1960;
        const int maxYear = 2026;
        return Math.Clamp((float)(year - minYear) / (maxYear - minYear), 0f, 1f);
    }

    /// <summary>
    /// Weighted Euclidean distance between a scored album and user preferences.
    /// 3D: Energy, Familiarity, Time (Emotion removed).
    /// Lower = closer match.
    /// </summary>
    public static double WeightedDistance(
        float albumEnergy, float albumFamiliarity, float albumTime,
        float reqEnergy,   float reqFamiliarity,   float reqTime)
    {
        var dE  = albumEnergy      - reqEnergy;
        var dF  = albumFamiliarity - reqFamiliarity;
        var dT  = albumTime        - reqTime;

        return Math.Sqrt(
            WeightEnergy      * dE  * dE  +
            WeightFamiliarity * dF  * dF  +
            WeightTime        * dT  * dT);
    }
}
