using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhoaLuan_QLNhaTro.Models
{
    public class DetailBill
    {
        public string idBill { get; set; }
        public Guid idService { get; set; }
        public int Number {  get; set; }
        public float Price { get; set; }
        public float Total {  get; set; }
        [ForeignKey("idBill"), Required]
        public Bill Bill { get; set; }
        [ForeignKey("idService"), Required]
        public Service Service { get; set; }
    }
}
