using CodeWright.Metadata.API.Queries.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeWright.Metadata.API.Queries;

/// <summary>
/// Database context for metadata views and queries
/// </summary>
public class MetadataDbContext : DbContext
{
    /// <summary>
    /// Create an instance of a MetadataDbContext.
    /// </summary>
    public MetadataDbContext(DbContextOptions<MetadataDbContext> options)
        : base(options) { }

    /// <summary>
    /// Metadata table
    /// </summary>
    public DbSet<MetadataEntity> Metadata { get; set; } = null!;

    /// <summary>
    /// References table
    /// </summary>
    public DbSet<ReferenceEntity> References { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MetadataEntity>().HasKey(p => new { p.Id, p.TenantId, p.Name });
        modelBuilder.Entity<MetadataEntity>().HasIndex(p => new { p.Id, p.TenantId });
        // Index for metadata search
        modelBuilder.Entity<MetadataEntity>().HasIndex(p => new { p.TenantId, p.Name, p.Value });

        modelBuilder.Entity<ReferenceEntity>().HasKey(p => new { p.Id, p.TenantId, p.Type, p.TargetId });
        modelBuilder.Entity<ReferenceEntity>().HasIndex(p => new { p.Id, p.TenantId });
    }
}
