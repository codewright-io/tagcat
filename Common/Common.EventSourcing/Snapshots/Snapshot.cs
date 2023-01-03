namespace CodeWright.Common.EventSourcing.Snapshots;

/// <summary>
/// A snapshot wrapping a domain object built from events
/// </summary>
/// <typeparam name="TModel">The type of domain object</typeparam>
public class Snapshot<TModel> where TModel : IDomainObject
{
    /// <summary>
    /// Create an instance of a Snapshot
    /// </summary>
    public Snapshot(TModel model, long version)
    {
        Model = model;
        Version = version;
    }

    /// <summary>
    /// The domain object
    /// </summary>
    public TModel Model { get; }

    /// <summary>
    /// The version of the last event processed by the snapshot
    /// </summary>
    public long Version { get; }
}
