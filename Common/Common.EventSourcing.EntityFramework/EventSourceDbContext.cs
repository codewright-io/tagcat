using CodeWright.Common.EventSourcing.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeWright.Common.EventSourcing.EntityFramework
{
    public class EventSourceDbContext : DbContext
    {
        public EventSourceDbContext(DbContextOptions<EventSourceDbContext> options)
            : base(options) { }

        public DbSet<EventLogEntity> Events { get; set; } = null!;

        public DbSet<SnapshotEntity> Snapshots { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventLogEntity>().HasKey(p => new { p.Id, p.TenantId, p.Version });
            modelBuilder.Entity<EventLogEntity>().HasIndex(p => new { p.Id, p.TenantId });
            modelBuilder.Entity<EventLogEntity>().HasIndex(p => new { p.Version });

            modelBuilder.Entity<SnapshotEntity>().HasKey(p => new { p.Id, p.TenantId });
        }
    }
}
