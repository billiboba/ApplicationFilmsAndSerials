using ApplicationFilmsAndSerials.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<FilmsAndSerialsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FilmsAndSerialsDatabase")));

var app = builder.Build();

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
