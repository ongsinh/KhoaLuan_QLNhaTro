using KhoaLuan_QLNhaTro.Models;
using Microsoft.EntityFrameworkCore;

namespace KhoaLuan_QLNhaTro.DAO
{
    public class RoomDAO
    {
        private readonly DataContext _dataContext;

        public RoomDAO (DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Room>> GetAll()
        {
            return await _dataContext.Rooms.ToListAsync<Room>();
        }


    }
}
