using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MyDockerProject;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
        connectionString = string.Format(connectionString, password);
        options.UseSqlServer(connectionString);
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Veritaban�n� ve tablolar� olu�tur (E�er yoksa)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Veritaban�n� ve tablolar� kontrol et, yoksa olu�tur
    if (!dbContext.Database.CanConnect()) // Ba�lant�y� kontrol et, e�er m�mk�nse veritaban� yok demektir
    {
        dbContext.Database.EnsureCreated(); // Veritaban� ve tablolar� olu�tur
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
