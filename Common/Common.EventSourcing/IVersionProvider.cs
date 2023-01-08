namespace CodeWright.Common.EventSourcing;

/// <summary>
/// Interface for a class that can generate incrementing version numbers
/// </summary>
public interface IVersionProvider
{
    /// <summary>
    /// Generate a new version number
    /// </summary>
    /// <returns></returns>
    long GetNewVersion();
}
