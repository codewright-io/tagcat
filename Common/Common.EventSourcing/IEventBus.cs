namespace CodeWright.Common.EventSourcing;

public interface IEventBus
{
    Task SendAsync(IEnumerable<IDomainEvent> events);
}
