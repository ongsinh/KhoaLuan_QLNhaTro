using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KhoaLuan_QLNhaTro.Models
{
    [PrimaryKey(nameof(ContractId), nameof(AssetId), nameof(ServiceId))]
    public class DetailContract
    {
        public string ContractId { get; set; }
        public Guid AssetId { get; set; }
        public Guid ServiceId { get; set; }
        public int Number {  get; set; }
        public float Price { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Asset Asset { get; set; }
        public virtual Service Service { get; set; }
    }
}
