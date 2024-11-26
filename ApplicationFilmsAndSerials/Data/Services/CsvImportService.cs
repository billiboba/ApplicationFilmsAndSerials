using System.Globalization;
using System.IO;
using CsvHelper;
using ApplicationFilmsAndSerials.Data;
using ApplicationFilmsAndSerials.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;

public class CsvImportService
{
    private readonly FilmsAndSerialsContext _context;

    public CsvImportService(FilmsAndSerialsContext context)
    {
        _context = context;
    }

    public void ImportFilmsFromCsv(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.UTF8);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        try
        {
            var csvRecords = csv.GetRecords<FilmCsvModel>().ToList();

            var csvTitles = csvRecords.Select(r => r.Title.Trim().ToLower()).ToHashSet();

            var dbFilms = _context.Films.Include(f => f.Genre).ToList();

            var filmsToRemove = dbFilms.Where(f => !csvTitles.Contains(f.Title.Trim().ToLower())).ToList();
            if (filmsToRemove.Any())
            {
                _context.Films.RemoveRange(filmsToRemove);
            }

            foreach (var record in csvRecords)
            {
                var genre = _context.Genres.FirstOrDefault(g => g.Name.Trim().ToLower() == record.Genre.Trim().ToLower())
                            ?? new Genre { Name = record.Genre.Trim() };

                if (genre.Id == 0)
                {
                    _context.Genres.Add(genre);
                    _context.SaveChanges();
                }

                var existingFilm = dbFilms.FirstOrDefault(f => f.Title.Trim().ToLower() == record.Title.Trim().ToLower());

                if (existingFilm == null)
                {
                    _context.Films.Add(new Films
                    {
                        Title = record.Title.Trim(),
                        ReleaseDate = record.ReleaseDate,
                        AgeLimit = record.AgeLimit,
                        Rating = record.Rating,
                        VideoPath = record.VideoPath,
                        GenreId = genre.Id
                    });
                    Console.WriteLine($"Добавлен фильм: {record.Title}");
                }
                else
                {
                    existingFilm.ReleaseDate = record.ReleaseDate;
                    existingFilm.AgeLimit = record.AgeLimit;
                    existingFilm.Rating = record.Rating;
                    existingFilm.VideoPath = record.VideoPath;
                    existingFilm.GenreId = genre.Id;

                    _context.Films.Update(existingFilm);
                    Console.WriteLine($"Обновлен фильм: {record.Title}");
                }
            }

            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при импорте фильмов: {ex.Message}");
        }
    }
    public void ImportSerialsFromCsv(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.UTF8);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<SerialsCsvModel>();
        foreach (var record in records)
        {
            try
            {
                var genre = _context.Genres.FirstOrDefault(g => g.Name == record.Genre);
                if (genre == null)
                {
                    Console.WriteLine($"Adding new genre: {record.Genre}");
                    genre = new Genre { Name = record.Genre };
                    _context.Genres.Add(genre);
                    _context.SaveChanges();
                }

                if (!_context.Serials.Any(s => s.Title == record.Title && s.SeasonNumber == record.SeasonNumber && s.EpisodeNumber == record.EpisodeNumber))
                {
                    var serial = new Serials
                    {
                        Title = record.Title,
                        ReleaseDate = record.ReleaseDate,
                        AgeLimit = record.AgeLimit,
                        Rating = record.Rating,
                        VideoPath = record.VideoPath,
                        GenreId = genre.Id,
                        SeasonNumber = record.SeasonNumber,
                        EpisodeNumber = record.EpisodeNumber
                    };

                    _context.Serials.Add(serial);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing record: {record.Title}, Season: {record.SeasonNumber}, Episode: {record.EpisodeNumber}. Error: {ex.Message}");
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

    public class SerialsCsvModel
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int AgeLimit { get; set; }
        public double Rating { get; set; }
        public string Genre { get; set; }
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public string VideoPath { get; set; }
    }
}
