using Latihan_Project.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// untuk menghubungkan ke database
builder.Services.AddDbContext<InventoryContext>(Options =>
    {
        Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

var app = builder.Build();

app.MapOpenApi();

app.UseSwaggerUI(Options =>
{
    Options.SwaggerEndpoint("/openapi/v1.json", "API v1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API JALAN SILAHKAN GUNAKAN /swagger");

app.Run();
