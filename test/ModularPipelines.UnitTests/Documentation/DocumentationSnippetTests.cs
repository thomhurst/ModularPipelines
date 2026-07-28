namespace ModularPipelines.UnitTests.Documentation;

public class DocumentationSnippetTests
{
    private static readonly HashSet<string> IntentionalLegacyDocumentation =
    [
        "RELEASE_NOTES_V3.md",
        "docs/docs/migrating-to-v3.md",
        "docs/docs/advanced/migrating-to-v2.md",
    ];

    [Test]
    public async Task Published_Getting_Started_Snippets_Use_Current_APIs()
    {
        var repositoryRoot = FindRepositoryRoot();

        foreach (var path in Directory.EnumerateFiles(repositoryRoot, "*.md", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(repositoryRoot, path).Replace('\\', '/');
            if (IntentionalLegacyDocumentation.Contains(relativePath)
                || relativePath.Split('/').Any(segment => segment is ".git" or "bin" or "node_modules" or "obj"))
            {
                continue;
            }

            var contents = await File.ReadAllTextAsync(path)
                .ConfigureAwait(false);

            await Assert.That(contents).DoesNotContain("PipelineHostBuilder.Create()");
            await Assert.That(contents).DoesNotContain("ExecuteAsync(IPipelineContext");
            await Assert.That(contents).DoesNotContain("await GetModule<");
        }
    }

    [Test]
    public async Task Readme_Installs_DotNet_Integration_Used_By_Its_Examples()
    {
        var readme = await File.ReadAllTextAsync(Path.Combine(FindRepositoryRoot(), "README_Template.md"))
            .ConfigureAwait(false);

        await Assert.That(readme).Contains("dotnet add package ModularPipelines.DotNet");
        await Assert.That(readme).Contains("var builder = Pipeline.CreateBuilder(args);");
        await Assert.That(readme).Contains("await builder.ExecutePipelineAsync();");
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "README_Template.md")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
               ?? throw new DirectoryNotFoundException("Could not find the repository root.");
    }
}
