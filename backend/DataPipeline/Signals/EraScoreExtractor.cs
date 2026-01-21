using DataPipeline.Raw;

namespace DataPipeline.Signals;

/// <summary>
/// Extracts temporal era signal from release year.
/// Maps year to a time period feeling:
/// - Very old (pre-1970) → vintage (low score)
/// - Old (1970-1990) → retro
/// - Recent (1990-2010) → contemporary
/// - Very recent (2010+) → modern (high score)
/// </summary>
public class EraScoreExtractor : ISignalExtractor
{
    public double Extract(RawAlbum album)
    {
        var year = album.Year;

        // Map year ranges to scores (0.0 = vintage, 1.0 = cutting edge modern)
        // Using a smooth curve rather than hard buckets
        
        if (year < 1960)
        {
            return 0.1; // Very vintage
        }
        else if (year < 1970)
        {
            return 0.2; // Vintage
        }
        else if (year < 1980)
        {
            return 0.3; // Classic era
        }
        else if (year < 1990)
        {
            return 0.4; // Retro
        }
        else if (year < 2000)
        {
            return 0.5; // Contemporary
        }
        else if (year < 2010)
        {
            return 0.6; // Recent
        }
        else if (year < 2015)
        {
            return 0.7; // Modern
        }
        else if (year < 2020)
        {
            return 0.8; // Very modern
        }
        else
        {
            return 0.9; // Cutting edge
        }
    }
}
