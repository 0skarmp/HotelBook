using Hotel.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// REGISTRAR EL dbContext con MySQL
builder.Services.AddDbContext<HotelContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("HotelConnection"),
        new MySqlServerVersion(new Version(8, 0, 26))));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();