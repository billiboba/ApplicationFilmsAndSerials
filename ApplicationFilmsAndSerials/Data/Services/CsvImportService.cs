using System.Globalization;
using System.IO;
using CsvHelper;
using ApplicationFilmsAndSerials.Data;
using ApplicationFilmsAndSerials.Models;

public class CsvImportService
{
    private readonly FilmsAndSerialsContext _context;

    public CsvImportService(FilmsAndSerialsContext context)
    {
        _context = context;
    }

    public void ImportFilmsFromCsv(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<FilmCsvModel>();
        foreach (var record in records)
        {
            var genre = _context.Genres.FirstOrDefault(g => g.Name == record.Genre);
            if (genre == null)
            {
                genre = new Genre { Name = record.Genre };
                _context.Genres.Add(genre);
                _context.SaveChanges();
            }

            if (!_context.Films.Any(f => f.Title == record.Title))
            {
                var film = new Films
                {
                    Title = record.Title,
                    ReleaseDate = record.ReleaseDate,
                    AgeLimit = record.AgeLimit,
                    Rating = record.Rating,
                    VideoPath = record.VideoPath,
                    GenreId = genre.Id
                };

                _context.Films.Add(film);
            }
        }

        _context.SaveChanges(); 
    }

    public class FilmCsvModel
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int AgeLimit { get; set; }
        public double Rating { get; set; }
        public string Genre { get; set; }
        public string VideoPath { get; set; }
    }
}
