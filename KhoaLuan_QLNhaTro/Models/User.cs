using System.ComponentModel.DataAnnotations;

namespace KhoaLuan_QLNhaTro.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CCCD { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
