using DataPipeline.Raw;

namespace DataPipeline.Signals;

/// <summary>
/// Extracts energy signal based on genre classification.
/// High-energy genres (Metal, Punk, EDM) → higher values.
/// Low-energy genres (Ambient, Classical, Folk) → lower values.
/// </summary>
public class GenreWeightExtractor : ISignalExtractor
{
    // Genre energy mappings (0.0 = very low energy, 1.0 = very high energy)
    private static readonly Dictionary<string, double> GenreEnergyMap = new(StringComparer.OrdinalIgnoreCase)
    {
        // Very High Energy (0.8 - 1.0)
        ["Metal"] = 0.95,
        ["Heavy Metal"] = 0.95,
        ["Death Metal"] = 1.0,
        ["Thrash Metal"] = 0.95,
        ["Punk"] = 0.9,
        ["Hardcore"] = 0.95,
        ["EDM"] = 0.85,
        ["Drum and Bass"] = 0.9,
        ["Techno"] = 0.85,
        ["Hardstyle"] = 0.95,
        
        // High Energy (0.6 - 0.8)
        ["Rock"] = 0.7,
        ["Hard Rock"] = 0.8,
        ["Alternative Rock"] = 0.65,
        ["Indie Rock"] = 0.65,
        ["Pop"] = 0.6,
        ["Hip Hop"] = 0.65,
        ["Rap"] = 0.7,
        ["Dance"] = 0.75,
        ["House"] = 0.7,
        
        // Medium Energy (0.4 - 0.6)
        ["Pop Rock"] = 0.55,
        ["Indie"] = 0.5,
        ["Alternative"] = 0.5,
        ["R&B"] = 0.5,
        ["Soul"] = 0.5,
        ["Funk"] = 0.6,
        ["Blues"] = 0.45,
        ["Country"] = 0.5,
        
        // Low Energy (0.2 - 0.4)
        ["Folk"] = 0.3,
        ["Acoustic"] = 0.3,
        ["Singer-Songwriter"] = 0.35,
        ["Jazz"] = 0.4,
        ["Classical"] = 0.3,
        ["Ambient"] = 0.15,
        ["Chillout"] = 0.25,
        ["Downtempo"] = 0.25,
        
        // Very Low Energy (0.0 - 0.2)
        ["Drone"] = 0.1,
        ["Meditation"] = 0.05,
        ["New Age"] = 0.2,
    };

    public double Extract(RawAlbum album)
    {
        // If no genres, return neutral
        if (album.Genres == null || album.Genres.Count == 0)
        {
            return 0.5;
        }

        // Calculate average energy from all matched genres
        var matchedEnergies = new List<double>();
        
        foreach (var genre in album.Genres)
        {
            if (GenreEnergyMap.TryGetValue(genre, out var energy))
            {
                matchedEnergies.Add(energy);
            }
        }

        // If no genres matched our map, return neutral
        if (matchedEnergies.Count == 0)
        {
            return 0.5;
        }

        // Return average of matched genre energies
        return matchedEnergies.Average();
    }
}
