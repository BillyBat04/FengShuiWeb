using Microsoft.EntityFrameworkCore;
using FengShuiWeb.Models;

namespace FengShuiWeb.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){ }
        public DbSet<User> Users { get; set; }
        public DbSet<RoomTip> RoomTips { get; set; }
        public DbSet<ZodiacRecommendation> ZodiacRecommendations { get; set; }
    }
}
