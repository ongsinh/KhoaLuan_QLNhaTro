using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KhoaLuan_QLNhaTro.Models
{
    [PrimaryKey(nameof(BillId), nameof(ServiceId))]
    public class DetailBill
    { 
        public string BillId { get; set; }
        public Guid ServiceId { get; set; }
        public int Number {  get; set; }
        public decimal Price { get; set; }
        public decimal Total {  get; set; }
        public decimal? OldNumber { get; set; }
        public decimal? NewNumber { get; set; }
        public virtual Bill Bill { get; set; }
        public virtual Service Service { get; set; }
    }
}
