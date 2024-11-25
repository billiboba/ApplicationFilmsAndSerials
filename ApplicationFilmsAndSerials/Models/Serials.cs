namespace ApplicationFilmsAndSerials.Models
{
    public class Serials
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CountOfEpisodes { get; set; }
        public int CountOfSeasons { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int AgeLimit { get; set; }
        public double Rating { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public string VideoPath { get; set; }
    }
}
