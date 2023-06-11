using CodeWright.Common.EventSourcing;
using CodeWright.Tagcat.API.Model;

namespace CodeWright.Tagcat.API.Events;

/// <summary>
/// Metadata was removed from an item
/// </summary>
public class ItemMetadataRemovedEvent : DomainEventBase, IDomainEvent
{
    /// <summary>The ID of the type of object that the domain event pertains to</summary>
    public override string TypeId { get; } = Item.DomainTypeId;

    /// <summary>
    /// A key/value list of metadata properties on the object.
    /// </summary>
    public IEnumerable<MetadataEntry> RemovedMetadata { get; init; } = Enumerable.Empty<MetadataEntry>();
}
