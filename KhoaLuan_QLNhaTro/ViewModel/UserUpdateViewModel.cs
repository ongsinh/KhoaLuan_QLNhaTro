namespace KhoaLuan_QLNhaTro.ViewModel
{
    public class UserUpdateViewModel
    {
        public Guid Id { get; set; } // ID người dùng để định danh.
        public string Name { get; set; }
        public string CCCD { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
