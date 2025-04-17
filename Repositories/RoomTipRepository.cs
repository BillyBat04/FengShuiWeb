using FengShuiWeb.Data;

namespace FengShuiWeb.Repositories
{
    public class RoomTipRepository : IRoomTipRepository
    {
        private readonly DataContext _context;
        public RoomTipRepository(DataContext context)
        {
            _context = context;
        }

        public string? GetTipByRoom(string roomName)
        {
            return _context.RoomTips
                .FirstOrDefault(r => r.RoomName.ToLower() == roomName.ToLower())?.Tip;
        }
    }
}
