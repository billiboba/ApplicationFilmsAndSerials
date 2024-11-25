namespace ApplicationFilmsAndSerials.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Films> Films { get; set; }
        public ICollection<Serials> Serials { get; set; }
    }
}
