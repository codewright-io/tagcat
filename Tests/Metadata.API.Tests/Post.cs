namespace CodeWright.Metadata.API.Tests
{
    public static class Post
    {
        public static string AddMetadata() => "api/items/metadata/v1/add";

        public static string RemoveMetadata() => "api/items/metadata/v1/remove";

        public static string SetMetadata() => "api/items/metadata/v1";

        public static string AddReferences() => "api/items/references/v1/add";

        public static string RemoveReferences() => "api/items/references/v1/remove";

        public static string SetReferences() => "api/items/references/v1";

        public static string AddTags() => "api/items/tags/v1/add";

        public static string RemoveTags() => "api/items/tags/v1/remove";

        public static string SetTags() => "api/items/tags/v1";
    }
}
