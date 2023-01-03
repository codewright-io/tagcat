using IdGen;

namespace CodeWright.Common.EventSourcing;

public class IdGenVersionProvider : IVersionProvider
{
    private readonly IdGenerator _idGenerator;

    public IdGenVersionProvider(IdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public long GetNewVersion() => _idGenerator.CreateId();
}
