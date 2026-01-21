using DataPipeline.Raw;
using DataPipeline.Enriched;
using DataPipeline.Signals;
using DataPipeline.Scoring;

namespace DataPipeline.Pipeline;

/// <summary>
/// Orchestrates the album enrichment pipeline.
/// This class has NO business logic - it only coordinates the flow:
/// 1. Extract signals from raw album
/// 2. Compute scores from signals
/// 3. Map scores to enums
/// 4. Build enriched album
/// 
/// This is a stateless, deterministic transformation.
/// </summary>
public class AlbumEnrichmentPipeline
{
    // Signal extractors
    private readonly GenreWeightExtractor _genreWeightExtractor;
    private readonly PopularityScoreExtractor _popularityScoreExtractor;
    private readonly LabelScaleExtractor _labelScaleExtractor;
    private readonly HarmonicDepthExtractor _harmonicDepthExtractor;
    private readonly ValenceDepthExtractor _valenceDepthExtractor;
    private readonly AcousticDepthExtractor _acousticDepthExtractor;
    private readonly EraScoreExtractor _eraScoreExtractor;
    private readonly TimelessnessExtractor _timelessnessExtractor;

    // Scorers
    private readonly EnergyScorer _energyScorer;
    private readonly EmotionScorer _emotionScorer;
    private readonly FamiliarityScorer _familiarityScorer;
    private readonly TimeScorer _timeScorer;

    public AlbumEnrichmentPipeline()
    {
        // Initialize all extractors
        _genreWeightExtractor = new GenreWeightExtractor();
        _popularityScoreExtractor = new PopularityScoreExtractor();
        _labelScaleExtractor = new LabelScaleExtractor();
        _harmonicDepthExtractor = new HarmonicDepthExtractor();
        _valenceDepthExtractor = new ValenceDepthExtractor();
        _acousticDepthExtractor = new AcousticDepthExtractor();
        _eraScoreExtractor = new EraScoreExtractor();
        _timelessnessExtractor = new TimelessnessExtractor();

        // Initialize all scorers
        _energyScorer = new EnergyScorer();
        _emotionScorer = new EmotionScorer();
        _familiarityScorer = new FamiliarityScorer();
        _timeScorer = new TimeScorer();
    }

    /// <summary>
    /// Enriches a raw album with psychological profile scores.
    /// Flow: Raw -> Signals -> Raw Score -> Enum (Decision) -> Normalized Score (Output).
    /// </summary>
    /// <param name="rawAlbum">Input raw album data</param>
    /// <returns>Enriched album with strict numeric scores</returns>
    public EnrichedAlbum Enrich(RawAlbum rawAlbum)
    {
        // ========== STEP 1: Extract Signals ==========
        var signals = ExtractSignals(rawAlbum);

        // ========== STEP 2: Compute Raw Scores ==========
        var rawEnergy = _energyScorer.ComputeScore(signals);
        var rawEmotion = _emotionScorer.ComputeScore(signals);
        var rawFamiliarity = _familiarityScorer.ComputeScore(signals);
        var rawTime = _timeScorer.ComputeScore(signals);

        // ========== STEP 3: Map to Internal Enums (Decision Layer) ==========
        var energyLevel = ScoreMapper.MapToEnergyLevel(rawEnergy);
        var emotionLevel = ScoreMapper.MapToEmotionLevel(rawEmotion);
        var familiarityLevel = ScoreMapper.MapToFamiliarityLevel(rawFamiliarity);
        var timeFeeling = ScoreMapper.MapToTimeFeeling(rawTime);

        // ========== STEP 4: Normalize to Output Scores (Quantization) ==========
        var finalEnergy = ScoreMapper.GetScore(energyLevel);
        var finalEmotion = ScoreMapper.GetScore(emotionLevel);
        var finalFamiliarity = ScoreMapper.GetScore(familiarityLevel);
        var finalTime = ScoreMapper.GetScore(timeFeeling);

        // ========== STEP 5: Build Output ==========
        return new EnrichedAlbum
        {
            Id = rawAlbum.Id,
            Title = rawAlbum.Title,
            Artist = rawAlbum.Artist,
            Year = rawAlbum.Year,
            CoverUrl = rawAlbum.CoverUrl,
            WikipediaUrl = rawAlbum.WikipediaUrl,
            EnergyScore = finalEnergy,
            EmotionScore = finalEmotion,
            FamiliarityScore = finalFamiliarity,
            TimeScore = finalTime
        };
    }

    /// <summary>
    /// Extracts all signals from raw album.
    /// Returns a dictionary of signal name to signal value.
    /// </summary>
    private Dictionary<string, double> ExtractSignals(RawAlbum rawAlbum)
    {
        return new Dictionary<string, double>
        {
            // Signals for Energy scoring
            ["GenreWeight"] = _genreWeightExtractor.Extract(rawAlbum),
            ["AudioEnergy"] = rawAlbum.Energy ?? 0.5, // Use raw energy or neutral

            // Signals for Emotion scoring
            ["ValenceDepth"] = _valenceDepthExtractor.Extract(rawAlbum),
            ["HarmonicDepth"] = _harmonicDepthExtractor.Extract(rawAlbum),

            // Signals for Familiarity scoring
            ["Popularity"] = _popularityScoreExtractor.Extract(rawAlbum),
            ["LabelScale"] = _labelScaleExtractor.Extract(rawAlbum),
            // Note: GenreWeight is already extracted above, reused here

            // Signals for Time scoring
            ["EraScore"] = _eraScoreExtractor.Extract(rawAlbum),
            ["Timelessness"] = _timelessnessExtractor.Extract(rawAlbum)
        };
    }
}
