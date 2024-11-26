using ApplicationFilmsAndSerials.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FilmsAndSerialsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FilmsAndSerialsDatabase")));
builder.Services.AddTransient<CsvImportService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<FilmsAndSerialsContext>();
        dbContext.Database.Migrate();
        Console.WriteLine("Database migration completed.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration error: {ex.Message}");
    }
}

using (var scope = app.Services.CreateScope())
{
    try
    {
        var csvService = scope.ServiceProvider.GetRequiredService<CsvImportService>();

        var csvPathFilms = Path.Combine("wwwroot", "CSVDataVideos", "movies.csv");
        if (File.Exists(csvPathFilms))
        {
            Console.WriteLine("Importing films from CSV...");
            csvService.ImportFilmsFromCsv(csvPathFilms);
        }

        var csvPathSerials = Path.Combine("wwwroot", "CSVDataVideos", "serials.csv");
        if (File.Exists(csvPathSerials))
        {
            Console.WriteLine("Importing serials from CSV...");
            csvService.ImportSerialsFromCsv(csvPathSerials);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"CSV import error: {ex.Message}");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MainPage}/{action=MainPage}/{id?}");

app.Run();
