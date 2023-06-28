using CodeWright.Common.EventSourcing;
using CodeWright.Tagcat.API.Events;

namespace CodeWright.Tagcat.API.Model
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
            if (events == null)
                return null;

            var filteredEvents = events.Where(ev => ev.TypeId == Item.DomainTypeId);
            if (!filteredEvents.Any())
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
                    case ItemRelationshipsAddedEvent addRelationshipsEvent:
                        item.AddRelationships(addRelationshipsEvent.AddedRelationships, addRelationshipsEvent.Version, addRelationshipsEvent.Time, addRelationshipsEvent.UserId, addRelationshipsEvent.SourceId);
                        break;
                    case ItemMetadataRemovedEvent removeMetadataEvent:
                        item.RemoveMetadata(removeMetadataEvent.RemovedMetadata, removeMetadataEvent.Version, removeMetadataEvent.Time, removeMetadataEvent.UserId, removeMetadataEvent.SourceId);
                        break;
                    case ItemRelationshipsRemovedEvent removeRelationshipsEvent:
                        item.RemoveRelationships(removeRelationshipsEvent.RemovedRelationships, removeRelationshipsEvent.Version, removeRelationshipsEvent.Time, removeRelationshipsEvent.UserId, removeRelationshipsEvent.SourceId);
                        break;
                    default:
                        throw new InvalidOperationException("Unrecognized event");
                }
            }

            return item;
        }
    }
}
