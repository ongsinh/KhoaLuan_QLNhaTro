﻿namespace KhoaLuan_QLNhaTro.Models.ViewModel
{
    public class HoaDonViewModel
    {
        public string billID { get; set; }
        public string NameRoom { get; set; }
        public decimal roomPrice { get; set; }
        public List<DetailHoaDonViewModel> Details { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
