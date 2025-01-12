using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalCarApp.Models
{
    public class Reservation
    {
        public int ID { get; set; }

        [Required]
        public int ClientID { get; set; }

        [ForeignKey("ClientID")]
        public Client? Client { get; set; }

        [Required]
        public int VehicleID { get; set; }

        [ForeignKey("VehicleID")]
        public Vehicle? Vehicle { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalCost { get; set; }

        // Dodanie właściwości obliczeniowej
        [NotMapped]
        public int NumberOfDays => (EndDate - StartDate).Days;
    }

}
