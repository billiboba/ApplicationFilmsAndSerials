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
    var csvService = scope.ServiceProvider.GetRequiredService<CsvImportService>();

    // ”кажите путь к вашему CSV файлу
    var csvPathFilms = Path.Combine("wwwroot", "CSVDataVideos", "movies.csv");
    if (File.Exists(csvPathFilms))
    {
        csvService.ImportFilmsFromCsv(csvPathFilms);
    }
    var csvPathSerials = Path.Combine("wwwroot", "CSVDataVideos", "serials.csv");
    if (File.Exists(csvPathSerials))
    {
        csvService.ImportSerialsFromCsv(csvPathSerials);
    }
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<FilmsAndSerialsContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration error: {ex.Message}");
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MainPage}/{action=MainPage}/{id?}");

app.Run();
