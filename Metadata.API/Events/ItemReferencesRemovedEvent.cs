using CodeWright.Common.EventSourcing;
using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Events;

/// <summary>
/// References were removed from an item
/// </summary>
public class ItemReferencesRemovedEvent : DomainEventBase, IDomainEvent
{
    /// <summary>The ID of the type of object that the domain event pertains to</summary>
    public override string TypeId { get; } = Item.DomainTypeId;

    /// <summary>
    /// A list of references or relationships that the object has to other objects.
    /// </summary>
    public IEnumerable<ReferenceEntry> RemovedReferences { get; init; } = Enumerable.Empty<ReferenceEntry>();
}
