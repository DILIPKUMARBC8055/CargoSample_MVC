using System.ComponentModel.DataAnnotations;

namespace CargoSample2.Models
{
    public class CargostatusViewModel
    {
        [Key]
        public int StatusId { get; set; }
        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }

    }
}
