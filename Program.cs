using Hotel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Registrar el DbContext con MySQL
builder.Services.AddDbContext<HotelContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("HotelConnection"),
        new MySqlServerVersion(new Version(8, 0, 26))));

// Agregar soporte para controladores y vistas (Razor)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); 
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 
app.UseRouting();
app.UseAuthorization();

// Rutas para Razor y controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); 

app.Run();
