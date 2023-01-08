using IdGen;

namespace CodeWright.Common.EventSourcing;

/// <summary>
/// Version provider that uses the IdGen library
/// </summary>
public class IdGenVersionProvider : IVersionProvider
{
    private readonly IdGenerator _idGenerator;

    /// <summary>
    /// Create an instance of the IdGenVersionProvider
    /// </summary>
    /// <param name="idGenerator"></param>
    public IdGenVersionProvider(IdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }

    /// <inheritdoc/>
    public long GetNewVersion() => _idGenerator.CreateId();
}
