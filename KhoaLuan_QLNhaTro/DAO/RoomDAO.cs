using KhoaLuan_QLNhaTro.Models;

namespace KhoaLuan_QLNhaTro.DAO
{
    public class RoomDAO
    {
        private readonly DataContext _dataContext;

        public RoomDAO (DataContext dataContext)
        {
            _dataContext = dataContext;
        }


    }
}
