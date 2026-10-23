using ModularPipelines.Build.Modules.UnitTests;
using ModularPipelines.Build.Settings;

namespace ModularPipelines.Build.UnitTests;

public class UnitTestProjectCoverageTests
{
    [Test]
    public async Task Pipeline_Registers_Every_Unit_Test_Project_Exactly_Once()
    {
        var repositoryRoot = FindRepositoryRoot();
        // These projects run in dedicated workflow jobs: Build in fast-fail, Cmd on Windows.
        string[] dedicatedJobProjects = ["ModularPipelines.Build.UnitTests.csproj", "ModularPipelines.Cmd.UnitTests.csproj"];
        var expected = Directory.EnumerateDirectories(Path.Combine(repositoryRoot, "test"), "*.UnitTests")
            .SelectMany(directory => Directory.EnumerateFiles(directory, "*.UnitTests.csproj"))
            .Select(Path.GetFileName)
            .Except(dedicatedJobProjects, StringComparer.Ordinal)
            .ToArray();
        var registered = UnitTestProjects.ModuleTypes
            .Select(type => (RunUnitTestModule) Activator.CreateInstance(type, Microsoft.Extensions.Options.Options.Create(new PipelineSettings()))!)
            .Select(module => module.BuildOutputProjectFileName)
            .ToArray();

        await Assert.That(registered.Length).IsEqualTo(registered.Distinct(StringComparer.Ordinal).Count());
        await Assert.That(string.Join('\n', expected.Except(registered).Order(StringComparer.Ordinal))).IsEqualTo(string.Empty);

        var projectFiles = Directory.EnumerateFiles(repositoryRoot, "*.csproj", SearchOption.AllDirectories)
            .ToLookup(Path.GetFileName, StringComparer.Ordinal);

        foreach (var project in registered)
        {
            await Assert.That(projectFiles[project].Count()).IsEqualTo(1);
        }
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "BuildSolutions.txt")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not find the repository root.");
    }
}
