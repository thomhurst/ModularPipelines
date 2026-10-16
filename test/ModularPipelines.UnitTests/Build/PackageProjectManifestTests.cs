using System.Xml.Linq;

namespace ModularPipelines.UnitTests.Build;

public class PackageProjectManifestTests
{
    [Test]
    public async Task Manifest_Contains_Every_Shippable_Project_Exactly_Once()
    {
        var repositoryRoot = FindRepositoryRoot();
        var manifest = File.ReadLines(Path.Combine(repositoryRoot, "PackProjects.txt"))
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith('#'))
            .ToArray();

        // Analyzer projects (including the legacy package wrapper) are bundled in core.
        // Template fixtures, examples, and the build host are not release packages.
        string[] excludedDirectories = ["bin", "obj", "templates", "ModularPipelines.Analyzers"];
        string[] bundledOrInternalProjects = ["ModularPipelines.Build", "ModularPipelines.Examples", "ModularPipelines.SourceGenerator"];
        var expected = Directory.EnumerateFiles(Path.Combine(repositoryRoot, "src"), "*.csproj", SearchOption.AllDirectories)
            .Where(path => !Path.GetRelativePath(repositoryRoot, path).Split(Path.DirectorySeparatorChar)
                .Any(part => excludedDirectories.Contains(part, StringComparer.Ordinal)))
            .Where(path => !bundledOrInternalProjects.Contains(Path.GetFileNameWithoutExtension(path), StringComparer.Ordinal))
            .Where(path => !XDocument.Load(path).Descendants("IsPackable")
                .Any(property => string.Equals(property.Value.Trim(), "false", StringComparison.OrdinalIgnoreCase)))
            .Select(path => Path.GetRelativePath(repositoryRoot, path).Replace('\\', '/'))
            .Append("tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj")
            .ToHashSet(StringComparer.Ordinal);

        await Assert.That(manifest.Length).IsEqualTo(manifest.Distinct(StringComparer.Ordinal).Count());
        await Assert.That(string.Join('\n', expected.Except(manifest).Order(StringComparer.Ordinal))).IsEqualTo(string.Empty);
        await Assert.That(string.Join('\n', manifest.Except(expected).Order(StringComparer.Ordinal))).IsEqualTo(string.Empty);

        foreach (var project in manifest)
        {
            await Assert.That(File.Exists(Path.Combine(repositoryRoot, project))).IsTrue();
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
