using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class VaultCliScraperTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Shared_Traversal_Preserves_Operands_After_Flags(bool alternativeSynopsis)
    {
        var helpText = """
            Usage: vault audit disable -tls-skip-verify <PATH>

            Options:
                -tls-skip-verify    Skip TLS verification
                -address=<string>    Address of the Vault server
            """;
        if (alternativeSynopsis)
        {
            helpText = "Usage: vault audit disable -address <ADDRESS>\n" + helpText;
        }

        var scraper = new VaultCliScraper(
            new VaultHelpExecutor(helpText),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<VaultCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var leaf = commands.Single(command => command.FullCommand == "vault audit disable");
        var path = leaf.PositionalArguments.Single();
        await Assert.That(path.PropertyName).IsEqualTo("Path");
        await Assert.That(path.IsRequired).IsEqualTo(!alternativeSynopsis);
        await Assert.That(path.AssociatedOptionSwitch).IsNull();
        await Assert.That(leaf.Options.Single(option => option.PropertyName == "TlsSkipVerify").SwitchName)
            .IsEqualTo("-tls-skip-verify");
    }

    [Test]
    public async Task Value_Option_Placeholders_Remain_Excluded_From_Positionals()
    {
        const string helpText = """
            Usage: vault read -address <ADDRESS> <PATH>

            Options:
                -address=<string>    Address of the Vault server
            """;
        var command = await new TestVaultCliScraper().ParseGroup(["vault", "read"], helpText);
        await Assert.That(command!.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["Path"]);
        await Assert.That(command.Options.Single().SwitchName).IsEqualTo("-address");
    }

    [Test]
    public async Task Shared_Traversal_Preserves_Required_Subcommand()
    {
        var scraper = new VaultCliScraper(
            new VaultHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<VaultCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var audit = commands.Single(command => command.FullCommand == "vault audit");
        using (Assert.Multiple())
        {
            await Assert.That(audit.PositionalArguments[0].PropertyName).IsEqualTo("Subcommand");
            await Assert.That(audit.PositionalArguments[0].IsRequired).IsTrue();
            await Assert.That(commands.Select(command => command.FullCommand))
                .Contains("vault audit disable");
        }
    }

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

    private sealed class VaultHelpExecutor(string? leafHelp = null) : ICliCommandExecutor
    {
        private static readonly IReadOnlyDictionary<string, string> Responses =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["--help"] = """
                    Usage: vault <command> [args]

                    Other commands:
                        audit       Interact with audit devices
                    """,
                ["audit --help"] = """
                    Usage: vault audit <subcommand> [options] [args]

                    This command groups subcommands for interacting with Vault's audit devices.

                    Subcommands:
                        disable    Disables an audit device
                    """,
                ["audit disable --help"] = """
                    Usage: vault audit disable [options]

                    Options:
                        -address=<string>    Address of the Vault server
                    """,
            };

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            if (!Responses.TryGetValue(arguments, out var response))
            {
                throw new InvalidOperationException($"Unexpected invocation: {command} {arguments}");
            }

            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = arguments == "audit disable --help" && leafHelp is not null ? leafHelp : response,
                StandardError = string.Empty,
                ExitCode = 0,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
