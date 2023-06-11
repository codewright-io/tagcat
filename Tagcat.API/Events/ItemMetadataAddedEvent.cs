using CodeWright.Common.EventSourcing;
using CodeWright.Tagcat.API.Model;

namespace CodeWright.Tagcat.API.Events;

/// <summary>
/// Metadata was added to an item
/// </summary>
public class ItemMetadataAddedEvent : DomainEventBase, IDomainEvent
{
    /// <summary>The ID of the type of object that the domain event pertains to</summary>
    public override string TypeId { get; } = Item.DomainTypeId;

    /// <summary>
    /// A key/value list of metadata properties added to the object.
    /// </summary>
    public IEnumerable<MetadataEntry> AddedMetadata { get; init; } = Enumerable.Empty<MetadataEntry>();
}
