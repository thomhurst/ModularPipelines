using System.Runtime.CompilerServices;
using System.Text.Json;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests;

public class ToolCatalogTests
{
    [Test]
    public async Task RegisteredCatalog_ContainsEveryPlatformAndValidMetadata()
    {
        var entries = OptionsGeneratorCommand.CreateToolCatalog();

        await Assert.That(entries.Count).IsGreaterThan(40);
        await Assert.That(entries
                .Where(entry => entry.GenerationPlatform == CliGenerationPlatform.Windows)
                .Select(entry => entry.ToolName))
            .IsEquivalentTo(["choco", "winget"]);
        await Assert.That(entries.Single(entry => entry.ToolName == "npm").IncludeInGenerationMatrix)
            .IsFalse();
        await Assert.That(entries.Single(entry => entry.ToolName == "git").GenerateCommandFacade)
            .IsFalse();
        await Assert.That(entries
                .Where(entry => entry.ToolName != "git")
                .All(entry => entry.GenerateCommandFacade))
            .IsTrue();
        await Assert.That(entries
                .Where(entry => entry.ToolName != "npm")
                .All(entry => entry.IncludeInGenerationMatrix))
            .IsTrue();
        await Assert.That(entries.All(
            entry => entry.OutputDirectory == $"src/{entry.PackageName}")).IsTrue();
    }

    [Test]
    public async Task Create_OrdersAndProjectsRegisteredScrapers()
    {
        var entries = ToolCatalog.Create(
        [
            new FakeCliScraper("zeta", "Zeta", "src/ModularPipelines.Zeta"),
            new FakeCliScraper(
                "alpha",
                "Alpha",
                @"src\ModularPipelines.Alpha",
                CliGenerationPlatform.Windows),
        ]);

        await Assert.That(entries.Select(entry => entry.ToolName))
            .IsEquivalentTo(
                ["alpha", "zeta"],
                TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(entries[0].PackageName).IsEqualTo("ModularPipelines.Alpha");
        await Assert.That(entries[0].OutputDirectory).IsEqualTo("src/ModularPipelines.Alpha");
        await Assert.That(entries[0].GenerationPlatform).IsEqualTo(CliGenerationPlatform.Windows);
    }

    [Test]
    public async Task ToJson_UsesWorkflowFieldNames()
    {
        var entries = ToolCatalog.Create(
        [
            new FakeCliScraper("fake", "Fake", "src/ModularPipelines.Fake"),
        ]);

        using var document = JsonDocument.Parse(ToolCatalog.ToJson(entries));
        var tool = document.RootElement[0];

        await Assert.That(tool.GetProperty("toolName").GetString()).IsEqualTo("fake");
        await Assert.That(tool.GetProperty("packageName").GetString()).IsEqualTo("ModularPipelines.Fake");
        await Assert.That(tool.GetProperty("namespacePrefix").GetString()).IsEqualTo("Fake");
        await Assert.That(tool.GetProperty("outputDirectory").GetString())
            .IsEqualTo("src/ModularPipelines.Fake");
        await Assert.That(tool.GetProperty("generationPlatform").GetString()).IsEqualTo("linux");
        await Assert.That(tool.GetProperty("includeInGenerationMatrix").GetBoolean()).IsTrue();
        await Assert.That(tool.GetProperty("generateCommandFacade").GetBoolean()).IsTrue();
    }

    [Test]
    public async Task Create_RejectsDuplicateToolNames()
    {
        var duplicateScrapers = new ICliScraper[]
        {
            new FakeCliScraper("fake", "Fake", "src/ModularPipelines.Fake"),
            new FakeCliScraper("FAKE", "Other", "src/ModularPipelines.Other"),
        };

        await Assert.That(() => ToolCatalog.Create(duplicateScrapers))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Create_RejectsOutputOutsideSingleSrcProject()
    {
        var scraper = new FakeCliScraper("fake", "Fake", "generated/Fake");

        await Assert.That(() => ToolCatalog.Create([scraper]))
            .Throws<InvalidOperationException>();
    }

    private sealed class FakeCliScraper(
        string toolName,
        string namespacePrefix,
        string outputDirectory,
        CliGenerationPlatform generationPlatform = CliGenerationPlatform.Linux) : ICliScraper
    {
        public string ToolName => toolName;

        public string NamespacePrefix => namespacePrefix;

        public string TargetNamespace => $"ModularPipelines.{namespacePrefix}";

        public string OutputDirectory => outputDirectory;

        public CliGenerationPlatform GenerationPlatform => generationPlatform;

        public Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public async IAsyncEnumerable<CliCommandDefinition> ScrapeAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            yield break;
        }

        public CliToolDefinition CreateToolDefinition() => new()
        {
            ToolName = ToolName,
            NamespacePrefix = NamespacePrefix,
            TargetNamespace = TargetNamespace,
            OutputDirectory = OutputDirectory,
            Commands = [],
            Errors = [],
        };
    }
}
