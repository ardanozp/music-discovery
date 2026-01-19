using Microsoft.AspNetCore.Mvc;
using RecommendationApi.Models;

namespace RecommendationApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{
    private static readonly List<Album> _albums = new()
    {
        new Album
        {
            Id = 1,
            Title = "Abbey Road",
            Artist = "The Beatles",
            Year = 1969,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/4/42/Beatles_-_Abbey_Road.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Abbey_Road",
            Mood = "Happy"
        },
        new Album
        {
            Id = 2,
            Title = "The Dark Side of the Moon",
            Artist = "Pink Floyd",
            Year = 1973,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/3/3b/Dark_Side_of_the_Moon.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/The_Dark_Side_of_the_Moon",
            Mood = "Chill"
        },
        new Album
        {
            Id = 3,
            Title = "Kind of Blue",
            Artist = "Miles Davis",
            Year = 1959,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/9/9c/MilesDavisKindofBlue.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Kind_of_Blue",
            Mood = "Chill"
        },
        new Album
        {
            Id = 4,
            Title = "Back in Black",
            Artist = "AC/DC",
            Year = 1980,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/3/32/ACDC_Back_in_Black.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Back_in_Black",
            Mood = "Energetic"
        },
        new Album
        {
            Id = 5,
            Title = "Rumours",
            Artist = "Fleetwood Mac",
            Year = 1977,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/f/fb/FMacRumours.PNG",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Rumours_(album)",
            Mood = "Happy"
        }
    };

    [HttpPost]
    public IActionResult GetRecommendations([FromBody] RecommendationRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Mood))
        {
            return BadRequest("Mood is required.");
        }

        var recommendedAlbums = _albums
            .Where(a => a.Mood.Equals(request.Mood, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(new { albums = recommendedAlbums });
    }
}
