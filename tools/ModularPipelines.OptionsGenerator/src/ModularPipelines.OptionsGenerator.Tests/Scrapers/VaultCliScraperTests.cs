using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class VaultCliScraperTests
{
    [Test]
    public async Task Command_Group_Preserves_Subcommand_And_Optional_Args()
    {
        const string helpText = """
            Usage: vault audit <subcommand> [options] [args]

              This command groups subcommands for interacting with Vault's audit devices.

            Subcommands:
                disable    Disables an audit device
                enable     Enables an audit device
                list       Lists enabled audit devices
            """;
        var command = await new TestVaultCliScraper().ParseGroup(
            ["vault", "audit"],
            helpText);

        var subcommand = command!.PositionalArguments[0];
        var argument = command.PositionalArguments[1];
        using (Assert.Multiple())
        {
            await Assert.That(subcommand.PropertyName).IsEqualTo("Subcommand");
            await Assert.That(subcommand.IsRequired).IsTrue();
            await Assert.That(argument.PropertyName).IsEqualTo("Args");
            await Assert.That(argument.CSharpType).IsEqualTo("string?");
            await Assert.That(argument.IsRequired).IsFalse();
            await Assert.That(argument.Phase).IsEqualTo(CommandLinePhase.Passthrough);
        }
    }

    private sealed class TestVaultCliScraper : VaultCliScraper
    {
        public TestVaultCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<VaultCliScraper>.Instance)
        {
        }

        public Task<CliCommandDefinition?> ParseGroup(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }
}
