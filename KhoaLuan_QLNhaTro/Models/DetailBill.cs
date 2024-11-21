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
        public float Price { get; set; }
        public float Total {  get; set; }
        public float? OldNumber { get; set; }
        public float? NewNumber { get; set; }
        public virtual Bill Bill { get; set; }
        public virtual Service Service { get; set; }
    }
}
