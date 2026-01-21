using DataPipeline.Raw;

namespace DataPipeline.Signals;

/// <summary>
/// Base interface for all signal extractors.
/// Each extractor has a single responsibility: extract ONE signal from raw album data.
/// All extractors are pure functions (deterministic, no side effects).
/// </summary>
public interface ISignalExtractor
{
    /// <summary>
    /// Extracts a single signal value from raw album data.
    /// </summary>
    /// <param name="album">Raw album data</param>
    /// <returns>Signal value between 0.0 and 1.0. Returns 0.5 for missing/unavailable data.</returns>
    double Extract(RawAlbum album);
}
