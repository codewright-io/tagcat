namespace CodeWright.Common.EventSourcing
{
    /// <summary>
    /// A factory class that can create domain model objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDomainObjectFactory<T>
        where T : IDomainObject
    {
        /// <summary>
        /// Create the domain object from an ordered list of events.
        /// </summary>
        T? CreateFromEvents(IEnumerable<IDomainEvent> events);

        /// <summary>
        /// Update the domain object from an ordered list of events.
        /// </summary>
        T? UpdateFromEvents(T model, IEnumerable<IDomainEvent> events);
    }
}
