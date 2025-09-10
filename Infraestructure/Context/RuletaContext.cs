using Microsoft.EntityFrameworkCore;
using Ruleta.Domain.Entities;

namespace Ruleta.Infraestructure.Context
{
    public class RuletaContext : DbContext
    {
        public DbSet<Gambler> Gamblers { get; set; }

        public RuletaContext(DbContextOptions<RuletaContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gambler>(entity =>
            {
                entity.HasKey(e => e.Name);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Funds).HasColumnType("decimal(10,2)");
                entity.HasIndex(e => e.Name).IsUnique();

                entity.ToTable(t => t.HasCheckConstraint("CK_Gambler_Funds_Positive", "[Funds] >= 0"));
            });
        }
    }
}
