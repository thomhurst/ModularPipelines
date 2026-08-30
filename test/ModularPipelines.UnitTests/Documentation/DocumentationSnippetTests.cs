using ModularPipelines.Secrets;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
        "src/ModularPipelines/Secrets/SecretMaskingOptions.cs",
    ];

    [Test]
    public async Task Published_Getting_Started_Snippets_Use_Current_APIs()
    {
        var repositoryRoot = FindRepositoryRoot();

        foreach (var relativePath in await GetTrackedRegularMarkdownPathsAsync(repositoryRoot).ConfigureAwait(false))
        {
            if (IntentionalLegacyDocumentation.Contains(relativePath))
            {
                continue;
            }

            var contents = await File.ReadAllTextAsync(Path.Combine(repositoryRoot, relativePath))
                .ConfigureAwait(false);

            await Assert.That(contents).DoesNotContain("PipelineHostBuilder.Create()");
            await Assert.That(Regex.IsMatch(contents, @"\bPipelineStatus\b")).IsFalse();
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
        await Assert.That(readme).Contains("await builder.RunAsync();");
    }

    [Test]
    public async Task Readme_Module_Sharing_Fence_Matches_Compiled_Current_Api_Shape()
    {
        var repositoryRoot = FindRepositoryRoot();
        string[] currentApiShape =
        [
            "PublishModule : Module",
            "protected override async Task ExecuteAsync(",
        ];
        var compiledFixture = await File.ReadAllTextAsync(Path.Combine(
                repositoryRoot,
                "test/ModularPipelines.DocumentationSnippets/CurrentApiSnippets.cs"))
            .ConfigureAwait(false);

        foreach (var expected in currentApiShape)
        {
            await Assert.That(compiledFixture).Contains(expected);
        }

        foreach (var fileName in new[] { "README_Template.md", "README.md" })
        {
            var readme = await File.ReadAllTextAsync(Path.Combine(repositoryRoot, fileName))
                .ConfigureAwait(false);

            foreach (var expected in currentApiShape)
            {
                await Assert.That(readme).Contains(expected);
            }

            await Assert.That(readme).DoesNotContain("ExecuteModuleAsync(");
        }
    }

    [Test]
    public async Task Pipeline_Status_Documentation_Matches_Compiled_Current_Api_Shape()
    {
        var repositoryRoot = FindRepositoryRoot();
        var pipelineHost = await File.ReadAllTextAsync(Path.Combine(
                repositoryRoot,
                "docs/docs/how-to/pipeline-host.md"))
            .ConfigureAwait(false);
        var versionedPipelineHost = await File.ReadAllTextAsync(Path.Combine(
                repositoryRoot,
                "docs/versioned_docs/version-3.x/how-to/pipeline-host.md"))
            .ConfigureAwait(false);
        var testing = await File.ReadAllTextAsync(Path.Combine(
                repositoryRoot,
                "docs/docs/how-to/testing.md"))
            .ConfigureAwait(false);
        var azure = await File.ReadAllTextAsync(Path.Combine(
                repositoryRoot,
                "docs/docs/examples/azure-example.md"))
            .ConfigureAwait(false);
        var compiledFixture = await File.ReadAllTextAsync(Path.Combine(
                repositoryRoot,
                "test/ModularPipelines.DocumentationSnippets/CurrentApiSnippets.cs"))
            .ConfigureAwait(false);

        await Assert.That(pipelineHost).Contains("ModuleStatus.Failed");
        await Assert.That(pipelineHost).Contains("FailureMode = FailureMode.ContinueOnFailure");
        await Assert.That(pipelineHost).Contains("ThrowOnPipelineFailure = false");
        await Assert.That(versionedPipelineHost)
            .Contains("ExecutionMode = ExecutionMode.WaitForAllModules");
        await Assert.That(versionedPipelineHost).Contains("ThrowOnPipelineFailure = false");
        await Assert.That(testing).Contains("ThrowOnPipelineFailure = false");
        await Assert.That(testing).Contains("ModuleStatus.Succeeded");
        await Assert.That(azure).DoesNotContain("ValueOrDefault!");
        await Assert.That(azure).Contains(".Value.");

        await Assert.That(compiledFixture).Contains("ModuleStatus.Failed");
        await Assert.That(compiledFixture).Contains("FailureMode = FailureMode.ContinueOnFailure");
        await Assert.That(compiledFixture).Contains("ThrowOnPipelineFailure = false");
        await Assert.That(compiledFixture).Contains("ModuleStatus.Succeeded");
        await Assert.That(compiledFixture).Contains("buildResult.Value.OutputPath");
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

    private static async Task<IReadOnlyList<string>> GetTrackedRegularMarkdownPathsAsync(string repositoryRoot)
    {
        var startInfo = new ProcessStartInfo("git")
        {
            WorkingDirectory = repositoryRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add("ls-files");
        startInfo.ArgumentList.Add("--format=%(objectmode) %(path)");
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

        return [.. output
            .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Where(line => !line.StartsWith("120000 ", StringComparison.Ordinal))
            .Select(line => line[(line.IndexOf(' ') + 1)..])];
    }
}
