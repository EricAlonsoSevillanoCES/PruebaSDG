namespace WebApiPoblacion.Servicies
{
    public class AppConfiguration
    {
        // Clase para acceder con facilidad a los valores de appsettings
        private readonly IConfiguration _configuration;
        public string? DefaultConnection { get; }

        public AppConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            DefaultConnection = _configuration.GetConnectionString("defaultConnection");
        }
    }
}
