using CodeWright.Common.EventSourcing;
using CodeWright.Metadata.API.Events;

namespace CodeWright.Metadata.API.Model;

/// <summary>
/// The core domain object that metadata and references are attached to
/// </summary>
public class Item : DomainObjectBase
{
    /// <summary>A unique ID for the type</summary>
    public override string TypeId { get; } = DomainTypeId;

    /// <summary>A unique ID for the type</summary>
    public static string DomainTypeId { get; } = "item";

    /// <summary>
    /// A list of references or relationships that the object has to other objects.
    /// </summary>
    public IEnumerable<ReferenceEntry> References { get; private set; } = Enumerable.Empty<ReferenceEntry>();

    /// <summary>
    /// A key/value list of metadata properties on the object.
    /// </summary>
    public IEnumerable<MetadataEntry> Metadata { get; private set; } = Enumerable.Empty<MetadataEntry>();

    /// <summary>
    /// Add metadata to an item, ignoring any that already exist
    /// </summary>
    public void AddMetadata(IEnumerable<MetadataEntry> metadata, long version, DateTime time, string userId, string sourceId)
    {
        // Filter out any doubles and work out what is added, updated
        var addedItems = metadata.Except(Metadata).Distinct().ToList();
        var updatedItems = metadata.Except(addedItems).Distinct().ToList();
        var itemNames = metadata.Select(i => i.Name).ToList();

        Metadata = Metadata
            .Where(m => !itemNames.Contains(m.Name))
            .Concat(addedItems)
            .Concat(updatedItems)
            .Distinct()
            .ToList();

        // First remove any items being updated
        if (updatedItems.Any())
        {
            QueueEvent(new ItemMetadataRemovedEvent
            {
                Id = Id,
                TenantId = TenantId,
                RemovedMetadata = updatedItems,
                UserId = userId,
                SourceId = sourceId,
                Time = time,
                Version = version,
            });
        }

        if (addedItems.Any())
        {
            QueueEvent(new ItemMetadataAddedEvent
            {
                Id = Id,
                TenantId = TenantId,
                AddedMetadata = addedItems.Concat(updatedItems).ToList(),
                UserId = userId,
                SourceId = sourceId,
                Time = time,
                Version = version + 1,
            });
        }
    }

    /// <summary>
    /// Remove the specified metadata from an item
    /// </summary>
    public void RemoveMetadata(IEnumerable<MetadataEntry> metadata, long version, DateTime time, string userId, string sourceId)
    {
        // Filter out any doubles and work out what is removed
        var removeNames = metadata.Select(m => m.Name).Distinct().ToList();
        var removedItems = Metadata.Where(m => removeNames.Contains(m.Name)).ToList();
        Metadata = Metadata.Except(removedItems).Distinct().ToList();

        if (removedItems.Any())
        {
            QueueEvent(new ItemMetadataRemovedEvent
            {
                Id = Id,
                TenantId = TenantId,
                RemovedMetadata = removedItems,
                UserId = userId,
                SourceId = sourceId,
                Time = time,
                Version = version,
            });
        }
    }

    /// <summary>
    /// Set all the metadata on an item, replacing any existing metadata
    /// </summary>
    public void SetMetadata(IEnumerable<MetadataEntry> metadata, long version, DateTime time, string userId, string sourceId)
    {
        // Filter out any doubles and work out what is removed
        var removedItems = Metadata.Except(metadata).ToList();
        var addedItems = metadata.Except(Metadata).ToList();
        Metadata = new List<MetadataEntry>(metadata);

        if (removedItems.Any())
        {
            QueueEvent(new ItemMetadataRemovedEvent
            {
                Id = Id,
                TenantId = TenantId,
                RemovedMetadata = removedItems,
                UserId = userId,
                SourceId = sourceId,
                Time = time,
                Version = version,
            });
        }

        if (addedItems.Any())
        {
            QueueEvent(new ItemMetadataAddedEvent
            {
                Id = Id,
                TenantId = TenantId,
                AddedMetadata = addedItems,
                UserId = userId,
                SourceId = sourceId,
                Time = time,
                Version = version + 1,
            });
        }
    }

    /// <summary>
    /// Add references for an item, ignoring any that already exist
    /// </summary>
    public void AddReferences(IEnumerable<ReferenceEntry> references, long version, DateTime time, string userId, string sourceId)
    {
        // Filter out any doubles and work out what is added, updated
        var addedItems = references.Except(References).Distinct().ToList();

        References = References
            .Concat(addedItems)
            .Distinct()
            .ToList();

        if (addedItems.Any())
        {
            QueueEvent(new ItemReferencesAddedEvent
            {
                Id = Id,
                TenantId = TenantId,
                AddedReferences = addedItems,
                UserId = userId,
                SourceId = sourceId,
                Time = time,
                Version = version,
            });
        }
    }

    /// <summary>
    /// Remove the specified references from an item
    /// </summary>
    public void RemoveReferences(IEnumerable<ReferenceEntry> references, long version, DateTime time, string userId, string sourceId)
    {
        // Filter out any doubles and work out what is removed
        var removedItems = References.Intersect(references).ToList();
        References = References.Except(removedItems).Distinct().ToList();

        if (removedItems.Any())
        {
            QueueEvent(new ItemReferencesRemovedEvent
            {
                Id = Id,
                TenantId = TenantId,
                RemovedReferences = removedItems,
                UserId = userId,
                SourceId = sourceId,
                Time = time,
                Version = version,
            });
        }
    }

    /// <summary>
    /// Set all the references on an item, replacing any existing references
    /// </summary>
    public void SetReferences(IEnumerable<ReferenceEntry> references, long version, DateTime time, string userId, string sourceId)
    {
        // Filter out any doubles and work out what is removed
        var removedItems = References.Except(references).ToList();
        var addedItems = references.Except(References).ToList();
        References = new List<ReferenceEntry>(references);

        if (removedItems.Any())
        {
            QueueEvent(new ItemReferencesRemovedEvent
            {
                Id = Id,
                TenantId = TenantId,
                RemovedReferences = removedItems,
                UserId = userId,
                SourceId = sourceId,
                Time = time,
                Version = version,
            });
        }

        if (addedItems.Any())
        {
            QueueEvent(new ItemReferencesAddedEvent
            {
                Id = Id,
                TenantId = TenantId,
                AddedReferences = addedItems,
                UserId = userId,
                SourceId = sourceId,
                Time = time,
                Version = version + 1,
            });
        }
    }
}
