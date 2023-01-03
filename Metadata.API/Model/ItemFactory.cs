using CodeWright.Common.EventSourcing;
using CodeWright.Metadata.API.Events;

namespace CodeWright.Metadata.API.Model
{
    /// <summary>
    /// Create item domain object from events
    /// </summary>
    public class ItemFactory : IDomainObjectFactory<Item>
    {
        /// <summary>
        /// Create an item domain object from a list of events
        /// </summary>
        /// <param name="events">The list of events used to create the item.</param>
        /// <returns>The item domain model</returns>
        public Item? CreateFromEvents(IEnumerable<IDomainEvent> events)
            => FromEventsInternal(null, events);

        /// <summary>
        /// Update an item domain object from a list of events
        /// </summary>
        /// <param name="model">The item to update</param>
        /// <param name="events">The events that update the item</param>
        /// <returns>The item domain model</returns>
        public Item? UpdateFromEvents(Item model, IEnumerable<IDomainEvent> events)
            => FromEventsInternal(model, events);

        private static Item? FromEventsInternal(Item? model, IEnumerable<IDomainEvent> events)
        {
            if (events == null || !events.Any())
                return null;

            var item = model ?? new Item
            {
                Id = events.First().Id,
                TenantId = events.First().TenantId,
            };

            foreach (var ev in events)
            {
                switch (ev)
                {
                    case ItemMetadataAddedEvent addMetadataEvent:
                        item.AddMetadata(addMetadataEvent.AddedMetadata, addMetadataEvent.Version, addMetadataEvent.Time, addMetadataEvent.UserId, addMetadataEvent.SourceId);
                        break;
                    case ItemReferencesAddedEvent addReferenceEvent:
                        item.AddReferences(addReferenceEvent.AddedReferences, addReferenceEvent.Version, addReferenceEvent.Time, addReferenceEvent.UserId, addReferenceEvent.SourceId);
                        break;
                    case ItemMetadataRemovedEvent removeMetadataEvent:
                        item.RemoveMetadata(removeMetadataEvent.RemovedMetadata, removeMetadataEvent.Version, removeMetadataEvent.Time, removeMetadataEvent.UserId, removeMetadataEvent.SourceId);
                        break;
                    case ItemReferencesRemovedEvent removeReferencesEvent:
                        item.RemoveReferences(removeReferencesEvent.RemovedReferences, removeReferencesEvent.Version, removeReferencesEvent.Time, removeReferencesEvent.UserId, removeReferencesEvent.SourceId);
                        break;
                    default:
                        throw new InvalidOperationException("Unrecognised event");
                }
            }

            return item;
        }
    }
}
