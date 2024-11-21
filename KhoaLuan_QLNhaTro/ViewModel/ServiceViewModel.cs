using KhoaLuan_QLNhaTro.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KhoaLuan_QLNhaTro.ViewModel
{
    public class ServiceViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Unit { get; set; }
        public int Number { get; set; }
        public List<Guid> SelectedRooms { get; set; } // Danh sách phòng được chọn

        [BindNever] // Loại bỏ khỏi quá trình binding
        public List<Room> AllRooms { get; set; }
    }
}
