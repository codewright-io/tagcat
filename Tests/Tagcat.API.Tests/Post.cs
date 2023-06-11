namespace CodeWright.Tagcat.API.Tests
{
    public static class Post
    {
        public static string AddMetadata() => "api/items/metadata/v1/add";

        public static string RemoveMetadata() => "api/items/metadata/v1/remove";

        public static string SetMetadata() => "api/items/metadata/v1";

        public static string AddRelationships() => "api/items/relationships/v1/add";

        public static string RemoveRelationships() => "api/items/relationships/v1/remove";

        public static string SetReflationships() => "api/items/relationships/v1";

        public static string AddTags() => "api/items/tags/v1/add";

        public static string RemoveTags() => "api/items/tags/v1/remove";

        public static string SetTags() => "api/items/tags/v1";
    }
}
