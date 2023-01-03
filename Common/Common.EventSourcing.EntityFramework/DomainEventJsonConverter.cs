using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace CodeWright.Common.EventSourcing.EntityFramework
{
    public class DomainEventJsonConverter : JsonConverter
    {
        private readonly Dictionary<string, Type> _eventClassLookup;

        public DomainEventJsonConverter(IEnumerable<Assembly> domainEventAssemblies)
        {
            _eventClassLookup = domainEventAssemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IDomainEvent).IsAssignableFrom(t))
                .ToDictionary(t => t.Name, t => t);
        }

        public DomainEventJsonConverter(Assembly domainEventAssembly)
            : this(new List<Assembly> { domainEventAssembly })
        {
        }

        public override bool CanConvert(Type objectType) => typeof(IDomainEvent).IsAssignableFrom(objectType);

        public override bool CanRead { get => true; }

        public override bool CanWrite { get => false; }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var item = JObject.Load(reader);

            if (!item.TryGetValue("EventClass", out var entry))
                return null; // Can't deserialize

            var key = entry.Value<string>();
            if (key == null)
                return null;

            if (_eventClassLookup.TryGetValue(key, out var messageType))
            {
                return item.ToObject(messageType);
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
            => throw new NotSupportedException("Write not supported");
    }
}
