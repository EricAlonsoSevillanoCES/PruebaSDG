using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiPoblacion.Models;
using WebApiPoblacion.Servicies;
using WebApiTiempo;

namespace WebApiPoblacion.Controllers
{
    [ApiController]
    [Route("api/v1/data/country")]
    public class CountryController : ControllerBase
    {
        // Clase para definir los Endpints que usará el controlador
        private readonly ILogger<CountryController> _logger;
        private readonly CountryApiService _countryApiService;
        private readonly ApplicationDbContext _context;

        public CountryController(ILogger<CountryController> logger, CountryApiService countryApiService, ApplicationDbContext context)
        {
            _logger = logger;
            _countryApiService = countryApiService;
            _context = context;
        }

        // Endpoint para obtener los países desde la API
        [HttpGet("countries")]
        public async Task<ActionResult<List<Country>>> GetCountries()
        {
            try
            {
                var countries = await _countryApiService.GetCountries();
                if (countries != null)
                    return Ok(countries);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener paises: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // Endpoint para obtener los países desde la API y almacenarlos en la base de datos
        [HttpPost]
        public async Task<ActionResult> PostCountryData()
        {
            try
            {
                var countries = await _countryApiService.GetCountries();
                if (countries == null || !countries.Any())
                {
                    return NotFound("No se encontraron datos en la API");
                }

                foreach (var country in countries)
                {
                    if (country.name?.common == null)
                    {
                        _logger.LogWarning("El país recibido tiene un nombre nulo");
                        continue;
                    }

                    // Verifica si ya existe un país con el mismo nombre común
                    var existingCountry = await _context.Countries
                        .Include(c => c.name)
                        .FirstOrDefaultAsync(c => c.name.common == country.name.common);

                    if (existingCountry == null)
                    {
                        await _context.Countries.AddAsync(country);
                    }
                    else
                    {
                        existingCountry.population = country.population;
                        if (country.name.official != null)
                        {
                            existingCountry.name.official = country.name.official;
                        }
                        _context.Countries.Update(existingCountry);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok("Datos almacenados y/o actualizados correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al almacenar datos: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // Endpoint para descargar datos almacenados en la base de datos
        [HttpGet]
        public async Task<ActionResult<List<Country>>> GetCountryData()
        {
            try
            {
                var countries = await _context.Countries.ToListAsync();
                if (countries != null && countries.Count > 0)
                {
                    return Ok(countries);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener datos: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
