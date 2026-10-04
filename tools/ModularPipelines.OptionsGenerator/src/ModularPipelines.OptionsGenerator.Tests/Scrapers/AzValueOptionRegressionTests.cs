using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class AzValueOptionRegressionTests
{
    [Test]
    [Arguments("account-key", "Storage account key.")]
    [Arguments("sas-token", "A Shared Access Signature.")]
    [Arguments("connection-string", "Storage account connection string.")]
    [Arguments("suffix", "The tenant suffix in the registry login server.")]
    [Arguments("target-path", "Absolute destination on the deployment host.")]
    [Arguments("backup-retention", "Backup retention setting.")]
    [Arguments("404-document", "Static website error document.")]
    [Arguments("future-option", "An unfamiliar option without a placeholder.")]
    public async Task MissingPlaceholderDefaultsToValue(string name, string description)
    {
        var command = await new Scraper().Parse($"Command\n    az test run : Run.\nArguments\n    --{name} : {description}");
        var option = command!.Options.Single();
        await Assert.That(option.IsFlag).IsFalse();
        await Assert.That(option.CSharpType).IsEqualTo("string?");
    }

    [Test]
    public async Task ParserArityOverridesAmbiguousDescriptions()
    {
        const string help = """
            Command
                az test run : Run.
            Arguments
                --future-flag : Opaque action.
                --future-value : Force a particular value.
                --explicit-bool : Allowed values: false, true.
            __MODULAR_PIPELINES_AZ_ARGUMENT_FLAGS__:{"--future-flag":true,"--future-value":false,"--explicit-bool":false}
            """;
        var command = await new Scraper().Parse(help);
        await Assert.That(command!.Options.Single(x => x.SwitchName == "--future-flag").IsFlag).IsTrue();
        await Assert.That(command.Options.Single(x => x.SwitchName == "--future-value").IsFlag).IsFalse();
        var boolean = command.Options.Single(x => x.SwitchName == "--explicit-bool");
        await Assert.That(boolean.IsFlag).IsFalse();
        await Assert.That(boolean.CSharpType).IsEqualTo("bool?");
    }

    [Test]
    [Arguments("--error-document --error-document-path", null)]
    [Arguments("--name --resource-name -n", "-n")]
    [Arguments("--name -n --resource-name", "-n")]
    public async Task ShortFormOnlyContainsSingleDashAlias(string switches, string? shortForm)
    {
        var command = await new Scraper().Parse($"Command\n    az test run : Run.\nArguments\n    {switches} : A value.");
        await Assert.That(command!.Options.Single().ShortForm).IsEqualTo(shortForm);
    }

    [Test]
    public async Task IncompleteMetadataFailsInsteadOfGuessing()
    {
        await Assert.That(() => new Scraper().Parse("Command\n    az test run : Run.\nArguments\n    --unknown : Value.\n__MODULAR_PIPELINES_AZ_ARGUMENT_FLAGS__:{}"))
            .Throws<InvalidOperationException>();
    }

    [Test]
    [Arguments("{}")]
    [Arguments("null")]
    [Arguments("{invalid}")]
    public async Task InvalidMetadataAbortsProductionTraversal(string metadata)
    {
        var scraper = new TraversalScraper(new TraversalExecutor(metadata));
        async Task Act()
        {
            await foreach (var command in scraper.ScrapeAsync())
            {
                // Consume the public production path, including its worker and parse error handling.
            }
        }

        var exception = await Assert.That(() => Act().WaitAsync(TimeSpan.FromSeconds(10))).ThrowsException();
        await Assert.That(exception).IsNotTypeOf<TimeoutException>();
        await Assert.That(exception is InvalidOperationException or System.Text.Json.JsonException).IsTrue();
    }

    [Test]
    public async Task CompleteMetadataPreservesDiscoveredCommands()
    {
        var scraper = new TraversalScraper(new TraversalExecutor("{\"--unknown\":false}"));
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Select(command => command.FullCommand).Order())
            .IsEquivalentTo(["az broken", "az later"]);
        await Assert.That(commands.All(command => !command.Options.Single(option => option.SwitchName == "--unknown").IsFlag)).IsTrue();
    }

    private sealed class TraversalScraper(ICliCommandExecutor executor) : AzCliScraper(
        executor,
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<AzCliScraper>.Instance)
    {
        protected override int MaxParallelism => 1;
    }

    private sealed class TraversalExecutor(string metadata) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments, CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardError = string.Empty,
                StandardOutput = arguments == "--help"
                    ? "Group\n    az : Azure.\nCommands:\n    broken : Broken metadata.\n    later : Later command."
                    : $"Command\n    az {arguments.Split(' ')[0]} : Run.\nArguments\n    --unknown : Value.\n{AzCliMetadataExecutor.MetadataMarker}{metadata}",
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }

    private sealed class Scraper() : AzCliScraper(
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<AzCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string help) => ParseCommandAsync(["az", "test", "run"], help, CancellationToken.None);
    }
}
