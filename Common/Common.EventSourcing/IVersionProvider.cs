namespace CodeWright.Common.EventSourcing;

public interface IVersionProvider
{
    long GetNewVersion();
}
