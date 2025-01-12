using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentalCarApp.Models
{
    public class Payment
    {
        public int ID { get; set; }
        public int ReservationID { get; set; }
        public Reservation? Reservation { get; set; }
       
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Finished";
        public DateTime PaymentDate { get; set; }
    }

}
