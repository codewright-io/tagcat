namespace CodeWright.Metadata.API.Model;

/// <summary>
/// The type of relationship
/// </summary>
public enum RelationshipType
{
    /// <summary>
    /// No relationship type specified. Do not use.
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// The item is a child of another item
    /// </summary>
    ChildOf = 1,

    /// <summary>
    /// The item is an alias of another item
    /// </summary>
    AliasOf = 2,

    /// <summary>
    /// The item is equivalent to another item
    /// </summary>
    EquivalentTo = 3,

    /// <summary>
    /// The item is related to another item
    /// </summary>
    RelatedTo = 4,

    /// <summary>
    /// The item is a sub-category of another item
    /// </summary>
    SubcategoryOf = 5,

    /// <summary>
    /// The item is translation of another item
    /// </summary>
    TranslationOf = 6,

    /// <summary>
    /// The item is a tag
    /// </summary>
    Tag = 7,
}
