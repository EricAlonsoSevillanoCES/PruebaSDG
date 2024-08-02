using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApiPoblacion.Servicies;
using WebApiTiempo;

var builder = WebApplication.CreateBuilder(args);
AppConfiguration config = new(builder.Configuration);

builder.Logging.ClearProviders();
builder.Logging.AddLog4Net();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.DefaultConnection,
            sqlServerOptions => sqlServerOptions.UseNetTopologySuite()
            ));

builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddSingleton(config); // Servicio Singleton para recibir la misma instacia siempre
builder.Services.AddScoped<CountryApiService>(); // Servicio Scoped para recibir la misma instancia en una ejecución

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPICountry", Version = "v1" });
});



var app = builder.Build();

// Configuracion HTTP request
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPICountry v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
