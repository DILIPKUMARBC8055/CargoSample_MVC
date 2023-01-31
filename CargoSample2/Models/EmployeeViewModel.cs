using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CargoSample2.Models
{
    public class EmployeeViewModel
    {

        [Key]
        public int EmpId { get; set; }
        [Required]
        public string UserName { get; set; }

        public string? EmpName { get; set; }

        [Range(1000000000, 9999999999,
           ErrorMessage = "Mobile no should be 10 digits")]
        public string EmpPhNo { get; set; }
        [Required]
        [EmailAddress]
        public string EmpEmail { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [DefaultValue("Empl@123")]
        public string? Password { get; set; }
        [Required]
        [DefaultValue(-1)]
        public int IsApproved { get; set; }
    }

    public class EmployeeLoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DefaultValue("Empl@123")]
        public string? Password { get; set; }
    }
}
