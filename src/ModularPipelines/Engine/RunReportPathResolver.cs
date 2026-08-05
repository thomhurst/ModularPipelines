using ModularPipelines.Distributed.Serialization;

namespace ModularPipelines.Engine;

internal sealed class RunReportPathResolver
{
    private readonly string _rootDirectory;

    public RunReportPathResolver()
        : this(FindRootDirectory(
            Directory.GetCurrentDirectory(),
            AppContext.BaseDirectory))
    {
    }

    internal RunReportPathResolver(string rootDirectory)
    {
        _rootDirectory = Path.GetFullPath(rootDirectory);
    }

    internal string Resolve(string path) => Path.GetFullPath(path, _rootDirectory);

    internal static string FindRootDirectory(
        string startingDirectory,
        string applicationDirectory) =>
        GitRootFinder.Find(startingDirectory) ?? Path.GetFullPath(applicationDirectory);
}
