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

// Veritabanýný ve tablolarý oluþtur (Eðer yoksa)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Veritabanýný ve tablolarý kontrol et, yoksa oluþtur
    if (!dbContext.Database.CanConnect()) // Baðlantýyý kontrol et, eðer mümkünse veritabaný yok demektir
    {
        dbContext.Database.EnsureCreated(); // Veritabaný ve tablolarý oluþtur
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
