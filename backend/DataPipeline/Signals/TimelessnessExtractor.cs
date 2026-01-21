using DataPipeline.Raw;

namespace DataPipeline.Signals;

/// <summary>
/// Extracts timelessness signal by combining genre characteristics and popularity.
/// Timeless albums:
/// - Have enduring genres (Classical, Jazz, Rock) rather than trendy ones (EDM, Trap)
/// - Have moderate-to-high popularity (proven staying power)
/// - Balance between being well-known but not overly commercial
/// </summary>
public class TimelessnessExtractor : ISignalExtractor
{
    // Genres that tend to be timeless vs. trendy
    private static readonly Dictionary<string, double> GenreTimelessnessMap = new(StringComparer.OrdinalIgnoreCase)
    {
        // Highly Timeless (0.8 - 1.0)
        ["Classical"] = 1.0,
        ["Jazz"] = 0.95,
        ["Blues"] = 0.9,
        ["Folk"] = 0.85,
        ["Rock"] = 0.85,
        ["Soul"] = 0.85,
        ["Funk"] = 0.8,
        
        // Moderately Timeless (0.5 - 0.8)
        ["Progressive Rock"] = 0.75,
        ["Art Rock"] = 0.75,
        ["Singer-Songwriter"] = 0.7,
        ["Country"] = 0.65,
        ["R&B"] = 0.65,
        ["Indie Rock"] = 0.6,
        ["Alternative Rock"] = 0.6,
        ["Pop Rock"] = 0.55,
        
        // Less Timeless (0.3 - 0.5)
        ["Pop"] = 0.45,
        ["Hip Hop"] = 0.4,
        ["Rap"] = 0.4,
        ["Electronic"] = 0.4,
        ["Indie"] = 0.5,
        ["Alternative"] = 0.5,
        
        // Trendy/Less Timeless (0.0 - 0.3)
        ["EDM"] = 0.25,
        ["Dubstep"] = 0.2,
        ["Trap"] = 0.15,
        ["Hyperpop"] = 0.1,
        ["Vaporwave"] = 0.2,
        ["Drill"] = 0.15,
    };

    public double Extract(RawAlbum album)
    {
        // Calculate genre timelessness
        var genreScore = CalculateGenreTimelessness(album.Genres);
        
        // Calculate popularity factor
        // Timeless albums are usually moderately-to-highly popular
        // Very low popularity = obscure, very high = overly commercial
        // Sweet spot is 40-80 popularity
        var popularityScore = CalculatePopularityTimelessness(album.Popularity);

        // Combine: 60% genre, 40% popularity
        var timelessness = (genreScore * 0.6) + (popularityScore * 0.4);

        return Math.Clamp(timelessness, 0.0, 1.0);
    }

    private double CalculateGenreTimelessness(List<string> genres)
    {
        if (genres == null || genres.Count == 0)
        {
            return 0.5; // Neutral if no genres
        }

        var matchedScores = new List<double>();
        
        foreach (var genre in genres)
        {
            if (GenreTimelessnessMap.TryGetValue(genre, out var score))
            {
                matchedScores.Add(score);
            }
        }

        // If no genres matched, assume moderate timelessness
        if (matchedScores.Count == 0)
        {
            return 0.5;
        }

        return matchedScores.Average();
    }

    private double CalculatePopularityTimelessness(int popularity)
    {
        // Optimal popularity for timelessness is 40-80
        // Below 40: too obscure to be timeless
        // Above 80: too commercial/trendy
        
        if (popularity < 20)
        {
            return 0.3; // Too obscure
        }
        else if (popularity < 40)
        {
            // Gradually increase from 0.3 to 0.7
            return 0.3 + ((popularity - 20) / 20.0 * 0.4);
        }
        else if (popularity <= 80)
        {
            // Sweet spot: high timelessness
            return 0.7 + ((popularity - 40) / 40.0 * 0.3); // 0.7 to 1.0
        }
        else
        {
            // Too commercial, decrease timelessness
            return 1.0 - ((popularity - 80) / 20.0 * 0.4); // 1.0 down to 0.6
        }
    }
}
