using ModularPipelines.FileSystem;

namespace ModularPipelines.DotNet.UnitTests;

internal static class TestProjectPaths
{
    private static readonly string RepositoryRoot = FindRepositoryRoot();

    public static FilePath CoreSolution { get; } = new(
        Path.Combine(RepositoryRoot, "ModularPipelines.slnx"));

    public static FilePath TestsForTestsProject { get; } = new(Path.Combine(
        RepositoryRoot,
        "test",
        "ModularPipelines.TestsForTests",
        "ModularPipelines.TestsForTests.csproj"));

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null
               && !System.IO.File.Exists(Path.Combine(directory.FullName, "ModularPipelines.slnx")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
               ?? throw new DirectoryNotFoundException("Could not find the repository root.");
    }
}
