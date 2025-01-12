using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentalCarApp.Models
{
    public class Vehicle
    {
        public int ID { get; set; }

        [Required]
        public string? Brand { get; set; }

        [Required]
        public string? Model { get; set; }
        public int Year { get; set; }
        [Required]
        public string? RegistrationNumber { get; set; }
       
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DailyPrice { get; set; }
        public string Status { get; set; } = "Free";
        public string DisplayName => $"{Brand} {Model}";

    }

}
