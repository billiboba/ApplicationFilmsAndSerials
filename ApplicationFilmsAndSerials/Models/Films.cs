namespace ApplicationFilmsAndSerials.Models
{
    public class Films
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int AgeLimit { get; set; }
        public double Rating { get; set; }
        public string VideoPath { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
