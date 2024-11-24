using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhoaLuan_QLNhaTro.Models
{
    public class Bill
    {
        [Key]
        public string Id { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}
