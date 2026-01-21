namespace DataPipeline;

/// <summary>
/// Represents the energy intensity level of an album.
/// Derived from EnergyScore (0.0 - 1.0).
/// Internal decision layer only.
/// </summary>
public enum EnergyLevel
{
    Low,      // 0.0 - 0.33
    Mid,      // 0.34 - 0.66
    High      // 0.67 - 1.0
}

/// <summary>
/// Represents the emotional depth of an album.
/// Derived from EmotionScore (0.0 - 1.0).
/// Internal decision layer only.
/// </summary>
public enum EmotionLevel
{
    Light,    // 0.0 - 0.5 (lighter, happier emotions)
    Deep      // 0.5 - 1.0 (deeper, more complex emotions)
}

/// <summary>
/// Represents how familiar or exploratory an album is.
/// Derived from FamiliarityScore (0.0 - 1.0).
/// Internal decision layer only.
/// </summary>
public enum FamiliarityLevel
{
    Exploratory,  // 0.0 - 0.5 (niche, unknown, adventurous)
    Familiar      // 0.5 - 1.0 (well-known, mainstream)
}

/// <summary>
/// Represents the temporal feeling or era association of an album.
/// Derived from TimeScore (0.0 - 1.0).
/// Internal decision layer only.
/// </summary>
public enum TimeFeeling
{
    Past,      // 0.0 - 0.33 (vintage, classic)
    Timeless,  // 0.34 - 0.66 (transcends time)
    Now        // 0.67 - 1.0 (modern, contemporary)
}

/// <summary>
/// Represents the type of record label.
/// Internal decision layer only.
/// </summary>
public enum LabelType
{
    Unknown,
    Indie,
    Major
}
