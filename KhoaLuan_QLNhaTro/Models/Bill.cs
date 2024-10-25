using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KhoaLuan_QLNhaTro.Models
{
    public class Bill
    {
        [Key, Column("idBill")]
        public string Id { get; set; }
        public string Status { get; set; }
        public float Total { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }


        public Guid idRoom { get; set; } // Khóa ngoại đến Room
        [ForeignKey("idRoom"), Required]
        public Room Room { get; set; }

        public Guid idAccount { get; set; } // Khóa ngoại đến Account
        [ForeignKey("idAccount"), Required]
        public Account Account { get; set; }
    }
}
