using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KhoaLuan_QLNhaTro.Models
{
    public class Room
    {
        [Key, Column("idRoom"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }
        public int Number { get; set; }
        public float Acreage { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        [ForeignKey("idHouse"),Required]
        public House House { get; set; }
        [ForeignKey("idAccount"), Required]
        public Account Account { get; set; }


        public ICollection<Bill> Bills { get; set; } = new List<Bill>();
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}
