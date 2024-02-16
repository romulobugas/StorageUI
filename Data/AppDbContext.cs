// Data/AppDbContext.cs

using Microsoft.EntityFrameworkCore;
using StorageUI.Models;

namespace StorageUI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserApp> UserApp { get; set; }
        // Outros DbSets, se houver

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações adicionais do modelo, se necessário
        }
    }
}
