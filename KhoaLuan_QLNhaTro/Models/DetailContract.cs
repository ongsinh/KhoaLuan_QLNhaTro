using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KhoaLuan_QLNhaTro.Models
{
    [PrimaryKey(nameof(ContractId), nameof(AssetId))]
    public class DetailContract
    {
        public string ContractId { get; set; }
        public Guid AssetId { get; set; }
        public int Number {  get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Asset Asset { get; set; }
    }
}
