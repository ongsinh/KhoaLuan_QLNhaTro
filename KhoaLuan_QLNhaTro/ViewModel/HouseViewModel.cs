namespace KhoaLuan_QLNhaTro.ViewModel
{
    public class HouseViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int FloorCount { get; set; }
        public List<int> RoomsPerFloor { get; set; } // Danh sách số phòng cho từng tầng
    }

}
