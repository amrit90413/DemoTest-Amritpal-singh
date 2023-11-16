using System.ComponentModel.DataAnnotations;

namespace Demotest.Models
{
    public class Countries
    {
        [Key]
        public int Id { get; set; }
        public string countryName { get; set; }
    }
}
