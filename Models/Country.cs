using System.ComponentModel.DataAnnotations;

namespace WebApiPoblacion.Models
{
    public class Country
    {
        // Clase Country(País) con propiedades de nombre y poblacion
        public int Id { get; set; }
        public Name? name { get; set; }
        public int population { get; set; }
    }
}
