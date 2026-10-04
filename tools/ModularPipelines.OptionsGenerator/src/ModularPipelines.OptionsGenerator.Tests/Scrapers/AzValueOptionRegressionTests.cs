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

    private sealed class Scraper() : AzCliScraper(
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<AzCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string help) => ParseCommandAsync(["az", "test", "run"], help, CancellationToken.None);
    }
}
