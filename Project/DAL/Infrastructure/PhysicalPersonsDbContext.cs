using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared.Infrastructure
{
    public class PhysicalPersonsDbContext : DbContext
    {
        public PhysicalPersonsDbContext(DbContextOptions<PhysicalPersonsDbContext> options) : base(options) { }
        public DbSet<PhysicalPerson> PhysicalPersons { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Relation> Relation { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Relation>()
                .HasKey(e => new { e.FromId, e.ToId });

            modelBuilder.Entity<Relation>()
                .HasOne(e => e.RelatedFrom)
                .WithMany(e => e.RelatedTo)
                .HasForeignKey(e => e.FromId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Relation>()
                .HasOne(e => e.RelatedTo)
                .WithMany(e => e.RelatedFrom)
                .HasForeignKey(e => e.ToId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PhysicalPerson>()
                .HasMany(p => p.PhoneNumbers)
                .WithOne()
                .HasForeignKey(e => e.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
