using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demotest.Models
{
    public class States
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "StateName is required")]
        public string StateName { get; set; }

        [Required(ErrorMessage = "Country  is required")]
        [ForeignKey("Country")]
        public int CountryId { get; set; }
    }
}
