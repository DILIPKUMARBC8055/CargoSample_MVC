using System.ComponentModel.DataAnnotations;

namespace CargoSample2.Models
{
    public class CargoType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        //Weight in Kgs
        public string Weight { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        [DataType(DataType.Currency)]
        //Original price
        public string Price { get; set; }
        public string ExtraWeight { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [DataType(DataType.Currency)]
        //Extra charges for additional weight
        public string ExtraPrice { get; set; }
    }
}
