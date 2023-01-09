using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CodeWright.Metadata.API.Tests
{
    public static class Get
    {
        public static string SwaggerIndex() => "/swagger/index.html";

        public static string ItemMetadata(string tenantId, string id) => $"api/items/metadata/v1/{tenantId}/{id}";

        public static string ItemRelationships(string tenantId, string id) => $"api/items/relationships/v1/{tenantId}/{id}";

        public static string Referencing(string tenantId, string targetId) 
            => $"api/items/relationships/v1/referencing/{tenantId}/{targetId}";

        public static string ItemTags(string tenantId, string id) => $"api/items/tags/v1/{tenantId}/{id}";

        public static string ItemsByTag(string tenantId, string tag) => $"api/items/tags/v1/search/{tenantId}?tag={tag}";

        public static string ItemEvents(string tenantId, string id, int limit) => $"api/items/v1/events/{tenantId}/{id}?limit={limit}";

        public static string SearchMetadata(string tenantId, string name, string value, string secondaryName, string secondaryValue, int limit, int offset) 
            => $"api/items/metadata/v1/search/{tenantId}?name={name}&value={value}&secondaryName={secondaryName}&secondaryValue={secondaryValue}&limit={limit}&offset={offset}";

        public static string SearchMetadata(string tenantId, string name, string value, int limit, int offset)
            => $"api/items/metadata/v1/search/{tenantId}?name={name}&value={value}&limit={limit}&offset={offset}";
    }
}
