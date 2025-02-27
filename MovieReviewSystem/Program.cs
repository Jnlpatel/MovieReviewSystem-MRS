using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieReviewSystem.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MovieReviewSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieReviewSystemContext") ?? throw new InvalidOperationException("Connection string 'MovieReviewSystemContext' not found.")));

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(); // For .NET 6 and later


// Add logging
builder.Logging.ClearProviders(); // Optional: Clear default providers
builder.Logging.AddConsole(); // Add console logging
// Or add other logging providers (e.g., file logging)
// builder.Logging.AddFile("logs/myapp.txt");

var app = builder.Build();




// Enable Swagger in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Review API V1");
        //c.RoutePrefix = string.Empty; // Open Swagger UI at root URL
        c.RoutePrefix = "swagger";  // adjust this as needed

    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers(); // Ensure controllers are mapped

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();