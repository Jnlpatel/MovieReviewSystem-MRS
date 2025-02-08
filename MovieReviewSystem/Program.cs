using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieReviewSystem.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MovieReviewSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieReviewSystemContext") ?? throw new InvalidOperationException("Connection string 'MovieReviewSystemContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();




// Enable Swagger in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Review API V1");
        c.RoutePrefix = string.Empty; // Open Swagger UI at root URL
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllers(); // Ensure controllers are mapped

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
