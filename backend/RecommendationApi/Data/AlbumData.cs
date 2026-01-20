namespace RecommendationApi.Data;

using RecommendationApi.Models;

public static class AlbumData
{
    public static readonly List<Album> Albums = new()
    {
        // Classic Rock & Pop
        new Album
        {
            Id = 1,
            Title = "Abbey Road",
            Artist = "The Beatles",
            Year = 1969,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/4/42/Beatles_-_Abbey_Road.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Abbey_Road",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 2,
            Title = "The Dark Side of the Moon",
            Artist = "Pink Floyd",
            Year = 1973,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/3/3b/Dark_Side_of_the_Moon.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/The_Dark_Side_of_the_Moon",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Timeless
        },
        new Album
        {
            Id = 3,
            Title = "Kind of Blue",
            Artist = "Miles Davis",
            Year = 1959,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/9/9c/MilesDavisKindofBlue.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Kind_of_Blue",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 4,
            Title = "Back in Black",
            Artist = "AC/DC",
            Year = 1980,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/3/32/ACDC_Back_in_Black.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Back_in_Black",
            Energy = EnergyLevel.High,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 5,
            Title = "Rumours",
            Artist = "Fleetwood Mac",
            Year = 1977,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/f/fb/FMacRumours.PNG",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Rumours_(album)",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 6,
            Title = "Led Zeppelin IV",
            Artist = "Led Zeppelin",
            Year = 1971,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/2/26/Led_Zeppelin_-_Led_Zeppelin_IV.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Led_Zeppelin_IV",
            Energy = EnergyLevel.High,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 7,
            Title = "Thriller",
            Artist = "Michael Jackson",
            Year = 1982,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/5/55/Michael_Jackson_-_Thriller.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Thriller_(album)",
            Energy = EnergyLevel.High,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 8,
            Title = "The Wall",
            Artist = "Pink Floyd",
            Year = 1979,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/1/13/PinkFloydWallCoverOriginalNoText.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/The_Wall",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 9,
            Title = "Hotel California",
            Artist = "Eagles",
            Year = 1976,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/4/49/Hotelcalifornia.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Hotel_California",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 10,
            Title = "Nevermind",
            Artist = "Nirvana",
            Year = 1991,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/b/b7/NirvanaNevermindalbumcover.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Nevermind",
            Energy = EnergyLevel.High,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        
        // Jazz & Blues
        new Album
        {
            Id = 11,
            Title = "A Love Supreme",
            Artist = "John Coltrane",
            Year = 1965,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/0/06/A_Love_Supreme.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/A_Love_Supreme",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Timeless
        },
        new Album
        {
            Id = 12,
            Title = "Blue Train",
            Artist = "John Coltrane",
            Year = 1957,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/6/68/Blue_Train_%28album%29.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Blue_Train_(album)",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 13,
            Title = "Time Out",
            Artist = "The Dave Brubeck Quartet",
            Year = 1959,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/e/e4/Time_Out_%28album%29.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Time_Out_(album)",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Timeless
        },
        new Album
        {
            Id = 14,
            Title = "Mingus Ah Um",
            Artist = "Charles Mingus",
            Year = 1959,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/9/92/Mingus_Ah_Um.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Mingus_Ah_Um",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 15,
            Title = "The Blues and the Abstract Truth",
            Artist = "Oliver Nelson",
            Year = 1961,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/3/3e/The_Blues_and_the_Abstract_Truth.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/The_Blues_and_the_Abstract_Truth",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Timeless
        },
        
        // Electronic & Ambient
        new Album
        {
            Id = 16,
            Title = "Selected Ambient Works 85-92",
            Artist = "Aphex Twin",
            Year = 1992,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/9/9f/Selected_Ambient_Works_85-92.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Selected_Ambient_Works_85%E2%80%9392",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Timeless
        },
        new Album
        {
            Id = 17,
            Title = "Music for Airports",
            Artist = "Brian Eno",
            Year = 1978,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/4/4b/Music_for_Airports.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Ambient_1:_Music_for_Airports",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Timeless
        },
        new Album
        {
            Id = 18,
            Title = "Discovery",
            Artist = "Daft Punk",
            Year = 2001,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/2/27/Daft_Punk_-_Discovery.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Discovery_(Daft_Punk_album)",
            Energy = EnergyLevel.High,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 19,
            Title = "Random Access Memories",
            Artist = "Daft Punk",
            Year = 2013,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/a/a7/Random_Access_Memories.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Random_Access_Memories",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Now
        },
        new Album
        {
            Id = 20,
            Title = "Homework",
            Artist = "Daft Punk",
            Year = 1997,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/9/9c/Daft_Punk_Homework.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Homework_(Daft_Punk_album)",
            Energy = EnergyLevel.High,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        
        // Hip-Hop & R&B
        new Album
        {
            Id = 21,
            Title = "Illmatic",
            Artist = "Nas",
            Year = 1994,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/2/27/IllmaticNas.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Illmatic",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 22,
            Title = "The Miseducation of Lauryn Hill",
            Artist = "Lauryn Hill",
            Year = 1998,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/5/5f/Lauryn_Hill_-_The_Miseducation_of_Lauryn_Hill.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/The_Miseducation_of_Lauryn_Hill",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 23,
            Title = "To Pimp a Butterfly",
            Artist = "Kendrick Lamar",
            Year = 2015,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/f/f6/Kendrick_Lamar_-_To_Pimp_a_Butterfly.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/To_Pimp_a_Butterfly",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Now
        },
        new Album
        {
            Id = 24,
            Title = "good kid, m.A.A.d city",
            Artist = "Kendrick Lamar",
            Year = 2012,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/5/51/Kendrick_Lamar_-_Good_Kid%2C_M.A.A.D_City.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Good_Kid,_M.A.A.D_City",
            Energy = EnergyLevel.High,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Now
        },
        new Album
        {
            Id = 25,
            Title = "Channel Orange",
            Artist = "Frank Ocean",
            Year = 2012,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/2/28/Channel_ORANGE.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Channel_Orange",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Now
        },
        
        // Alternative & Indie
        new Album
        {
            Id = 26,
            Title = "OK Computer",
            Artist = "Radiohead",
            Year = 1997,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/b/ba/Radioheadokcomputer.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/OK_Computer",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Timeless
        },
        new Album
        {
            Id = 27,
            Title = "Kid A",
            Artist = "Radiohead",
            Year = 2000,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/0/02/Radioheadkida.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Kid_A",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Timeless
        },
        new Album
        {
            Id = 28,
            Title = "In Rainbows",
            Artist = "Radiohead",
            Year = 2007,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/1/14/Inrainbowscover.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/In_Rainbows",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 29,
            Title = "Is This It",
            Artist = "The Strokes",
            Year = 2001,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/0/09/Is_This_It_cover.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Is_This_It",
            Energy = EnergyLevel.High,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 30,
            Title = "Turn On the Bright Lights",
            Artist = "Interpol",
            Year = 2002,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/6/6c/Turn_On_the_Bright_Lights.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Turn_On_the_Bright_Lights",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Past
        },
        
        // Soul & Funk
        new Album
        {
            Id = 31,
            Title = "What's Going On",
            Artist = "Marvin Gaye",
            Year = 1971,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/b/b8/What%27s_Going_On.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/What%27s_Going_On_(album)",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Timeless
        },
        new Album
        {
            Id = 32,
            Title = "Songs in the Key of Life",
            Artist = "Stevie Wonder",
            Year = 1976,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/e/e2/Stevie_Wonder-Songs_in_the_key_of_life.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Songs_in_the_Key_of_Life",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Timeless
        },
        new Album
        {
            Id = 33,
            Title = "Super Fly",
            Artist = "Curtis Mayfield",
            Year = 1972,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/3/3e/Curtis_Mayfield_-_Super_Fly.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Super_Fly_(soundtrack)",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 34,
            Title = "There's a Riot Goin' On",
            Artist = "Sly and the Family Stone",
            Year = 1971,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/6/6c/Sly_%26_the_Family_Stone_-_There%27s_a_Riot_Goin%27_On.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/There%27s_a_Riot_Goin%27_On",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Past
        },
        new Album
        {
            Id = 35,
            Title = "Innervisions",
            Artist = "Stevie Wonder",
            Year = 1973,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/c/c4/Innervisions.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Innervisions",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Timeless
        },
        
        // Modern Pop & Alternative
        new Album
        {
            Id = 36,
            Title = "Blonde",
            Artist = "Frank Ocean",
            Year = 2016,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/a/a0/Blonde_-_Frank_Ocean.jpeg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Blonde_(Frank_Ocean_album)",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Now
        },
        new Album
        {
            Id = 37,
            Title = "Currents",
            Artist = "Tame Impala",
            Year = 2015,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/9/9b/Tame_Impala_-_Currents.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Currents_(Tame_Impala_album)",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Now
        },
        new Album
        {
            Id = 38,
            Title = "Melodrama",
            Artist = "Lorde",
            Year = 2017,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/b/b2/Lorde_-_Melodrama.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Melodrama_(Lorde_album)",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Now
        },
        new Album
        {
            Id = 39,
            Title = "After Hours",
            Artist = "The Weeknd",
            Year = 2020,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/c/c1/The_Weeknd_-_After_Hours.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/After_Hours_(The_Weeknd_album)",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Now
        },
        new Album
        {
            Id = 40,
            Title = "Folklore",
            Artist = "Taylor Swift",
            Year = 2020,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/f/f8/Taylor_Swift_-_Folklore.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Folklore_(Taylor_Swift_album)",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Now
        },
        
        // Experimental & Art Rock
        new Album
        {
            Id = 41,
            Title = "The Velvet Underground & Nico",
            Artist = "The Velvet Underground",
            Year = 1967,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/4/4b/The_Velvet_Underground_and_Nico.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/The_Velvet_Underground_%26_Nico",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Timeless
        },
        new Album
        {
            Id = 42,
            Title = "Loveless",
            Artist = "My Bloody Valentine",
            Year = 1991,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/3/36/My_Bloody_Valentine_-_Loveless.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Loveless_(album)",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Timeless
        },
        new Album
        {
            Id = 43,
            Title = "In the Aeroplane Over the Sea",
            Artist = "Neutral Milk Hotel",
            Year = 1998,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/4/4c/In_the_Aeroplane_Over_the_Sea_album_cover.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/In_the_Aeroplane_Over_the_Sea",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Timeless
        },
        new Album
        {
            Id = 44,
            Title = "Remain in Light",
            Artist = "Talking Heads",
            Year = 1980,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/8/8c/Talking_Heads_-_Remain_in_Light.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Remain_in_Light",
            Energy = EnergyLevel.High,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Timeless
        },
        new Album
        {
            Id = 45,
            Title = "The Rise and Fall of Ziggy Stardust",
            Artist = "David Bowie",
            Year = 1972,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/0/01/ZiggyStardust.jpg",
            WikipediaUrl = "https://en.wikipedia.org/wiki/The_Rise_and_Fall_of_Ziggy_Stardust_and_the_Spiders_from_Mars",
            Energy = EnergyLevel.High,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Past
        },
        
        // Contemporary & Diverse
        new Album
        {
            Id = 46,
            Title = "Immunity",
            Artist = "Clairo",
            Year = 2019,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/3/35/Clairo_-_Immunity.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Immunity_(Clairo_album)",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Light,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Now
        },
        new Album
        {
            Id = 47,
            Title = "Punisher",
            Artist = "Phoebe Bridgers",
            Year = 2020,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/9/9b/Phoebe_Bridgers_-_Punisher.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Punisher_(album)",
            Energy = EnergyLevel.Low,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Now
        },
        new Album
        {
            Id = 48,
            Title = "Fetch the Bolt Cutters",
            Artist = "Fiona Apple",
            Year = 2020,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/5/54/Fiona_Apple_-_Fetch_the_Bolt_Cutters.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Fetch_the_Bolt_Cutters",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Now
        },
        new Album
        {
            Id = 49,
            Title = "DAMN.",
            Artist = "Kendrick Lamar",
            Year = 2017,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/5/51/Kendrick_Lamar_DAMN.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Damn_(Kendrick_Lamar_album)",
            Energy = EnergyLevel.High,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Familiar,
            Time = TimeFeeling.Now
        },
        new Album
        {
            Id = 50,
            Title = "Hounds of Love",
            Artist = "Kate Bush",
            Year = 1985,
            CoverUrl = "https://upload.wikimedia.org/wikipedia/en/5/5f/Kate_Bush_Hounds_of_Love_1985.png",
            WikipediaUrl = "https://en.wikipedia.org/wiki/Hounds_of_Love",
            Energy = EnergyLevel.Mid,
            Emotion = EmotionLevel.Deep,
            Familiarity = FamiliarityLevel.Exploratory,
            Time = TimeFeeling.Timeless
        }
    };
}
