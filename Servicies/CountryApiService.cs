using Newtonsoft.Json;
using RestSharp;
using WebApiPoblacion.Models;

namespace WebApiPoblacion.Servicies
{
    public class CountryApiService
    {
        // Servicio para hacer llamadas a APIs usando RestSharp
        private readonly RestClient _restClient;
        private readonly ILogger<CountryApiService> _logger;

        public CountryApiService(ILogger<CountryApiService> logger)
        {
            _restClient = new RestClient("https://restcountries.com/v3.1/all");
            _logger = logger;
        }


        // Metodo para obtener todos los paises y sus datos requeridos,
        // usando DeserializeObject para obtener solo los necesarios
        public async Task<List<Country>> GetCountries()
        {
            try
            {
                var request = new RestRequest("",Method.Get);
                var response = await _restClient.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var countries = JsonConvert.DeserializeObject<List<Country>>(response.Content ?? "");
                    return countries ?? new List<Country>();
                }
                else
                {
                    _logger.LogError("Error en la petición de países: {StatusCode}", response.StatusCode);
                    return new List<Country>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al obtener datos de la API: {Message}", ex.Message);
                return new List<Country>();
            }
        }
    }
}
