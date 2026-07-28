namespace ModularPipelines.UnitTests.Documentation;

public class DocumentationSnippetTests
{
    private static readonly string[] PublishedDocumentationFiles =
    [
        "README.md",
        "README_Template.md",
        Path.Combine("docs", "docs", "how-to", "execution-and-dependencies.md"),
    ];

    [Test]
    public async Task Published_Getting_Started_Snippets_Use_Current_APIs()
    {
        var repositoryRoot = FindRepositoryRoot();

        foreach (var relativePath in PublishedDocumentationFiles)
        {
            var contents = await File.ReadAllTextAsync(Path.Combine(repositoryRoot, relativePath))
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
        await Assert.That(readme).Contains("builder.Services");
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
