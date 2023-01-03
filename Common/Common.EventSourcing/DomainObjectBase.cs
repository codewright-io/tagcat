namespace CodeWright.Common.EventSourcing
{
    public abstract class DomainObjectBase : IDomainObject
    {
        /// <summary>
        /// Unique identifier for the object
        /// </summary>
        public string Id { get; init; } = String.Empty;

        /// <summary>
        /// Tenant Id for the object
        /// </summary>
        public string TenantId { get; init; } = String.Empty;

        /// <summary>
        /// Version of the object
        /// </summary>
        public long Version { get; protected set; }

        /// <summary>A unique ID for the type</summary>
        public abstract string TypeId { get; }

        private readonly List<IDomainEvent> _eventQueue = new List<IDomainEvent>();
        private bool _queuing;

        public void QueueEvent(IDomainEvent ev)
        {
            if (_queuing)
            {
                _eventQueue.Add(ev);
            }

            Version = ev.Version;
        }

        /// <summary>
        /// Start queuing change events.
        /// </summary>
        public void StartQueuing()
        {
            _eventQueue.Clear();
            _queuing = true;
        }

        /// <summary>
        /// Stop queueing events and fetch the current list.
        /// </summary>
        public IEnumerable<IDomainEvent> StopQueuing()
        {
            var events = new List<IDomainEvent>(_eventQueue);
            _eventQueue.Clear();
            _queuing = false;
            return events;
        }
    }
}
