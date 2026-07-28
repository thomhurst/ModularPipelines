using System.Diagnostics;

namespace ModularPipelines.UnitTests.Documentation;

public class DocumentationSnippetTests
{
    private static readonly HashSet<string> IntentionalLegacyDocumentation =
    [
        "RELEASE_NOTES_V3.md",
        "docs/docs/migrating-to-v3.md",
        "docs/docs/advanced/migrating-to-v2.md",
        "docs/versioned_docs/version-3.x/migrating-to-v3.md",
    ];

    private static readonly string[] CurrentApiXmlDocumentation =
    [
        "src/ModularPipelines/Options/SecretMaskingOptions.cs",
        "src/ModularPipelines/Requirements/MacOSRequirement.cs",
        "src/ModularPipelines/Requirements/WindowsAdminRequirement.cs",
    ];

    [Test]
    public async Task Published_Getting_Started_Snippets_Use_Current_APIs()
    {
        var repositoryRoot = FindRepositoryRoot();

        foreach (var relativePath in await GetTrackedMarkdownPathsAsync(repositoryRoot).ConfigureAwait(false))
        {
            if (IntentionalLegacyDocumentation.Contains(relativePath))
            {
                continue;
            }

            var contents = await File.ReadAllTextAsync(Path.Combine(repositoryRoot, relativePath))
                .ConfigureAwait(false);

            await Assert.That(contents).DoesNotContain("PipelineHostBuilder.Create()");
            await Assert.That(contents).DoesNotContain("ExecuteAsync(IPipelineContext");
            await Assert.That(contents).DoesNotContain("await GetModule<");

            if (!relativePath.StartsWith("docs/versioned_docs/version-3.x/", StringComparison.Ordinal))
            {
                await AssertDoesNotUseRetiredBuilderApisAsync(contents);
            }
        }

        foreach (var relativePath in CurrentApiXmlDocumentation)
        {
            var contents = await File.ReadAllTextAsync(Path.Combine(repositoryRoot, relativePath))
                .ConfigureAwait(false);

            await AssertDoesNotUseRetiredBuilderApisAsync(contents);
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

    private static async Task AssertDoesNotUseRetiredBuilderApisAsync(string contents)
    {
        await Assert.That(contents).DoesNotContain("builder.Build().RunAsync()");
        await Assert.That(contents).DoesNotContain("builder.Services.AddModule<");
    }

    private static async Task<IReadOnlyList<string>> GetTrackedMarkdownPathsAsync(string repositoryRoot)
    {
        var startInfo = new ProcessStartInfo("git")
        {
            WorkingDirectory = repositoryRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add("ls-files");
        startInfo.ArgumentList.Add("--");
        startInfo.ArgumentList.Add("*.md");

        using var process = Process.Start(startInfo)
                            ?? throw new InvalidOperationException("Could not start git.");
        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync().ConfigureAwait(false);
        var output = await outputTask.ConfigureAwait(false);
        var error = await errorTask.ConfigureAwait(false);
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Could not list tracked Markdown files: {error}");
        }

        return output
            .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .ToArray();
    }
}
