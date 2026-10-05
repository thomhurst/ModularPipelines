using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class CliScraperTraversalTests
{
    [Test]
    [Arguments("helm")]
    [Arguments("kubectl")]
    [Arguments("terraform")]
    [Arguments("kustomize")]
    public async Task Tool_Adapters_Preserve_Discovered_Version_Subcommands(string tool)
    {
        var helpSwitch = tool == "terraform" ? "-help" : "--help";
        var section = tool == "terraform" ? "Main commands:" : "Available Commands:";
        var executor = new StubExecutor(new Dictionary<string, string>
        {
            [helpSwitch] = $"Usage: {tool} [command]\n\n{section}\n  version     Print the tool version\n",
            [$"version {helpSwitch}"] = $"Usage: {tool} version\n\nPrint the tool version.\n",
        });
        var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        CliScraperBase scraper = tool switch
        {
            "helm" => new HelmCliScraper(executor, cache, NullLogger<HelmCliScraper>.Instance),
            "kubectl" => new KubectlCliScraper(executor, cache, NullLogger<KubectlCliScraper>.Instance),
            "terraform" => new TerraformCliScraper(executor, cache, NullLogger<TerraformCliScraper>.Instance),
            "kustomize" => new KustomizeCliScraper(executor, cache, NullLogger<KustomizeCliScraper>.Instance),
            _ => throw new ArgumentOutOfRangeException(nameof(tool)),
        };
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Select(command => command.FullCommand)).IsEquivalentTo([$"{tool} version"]);
    }

    [Test]
    public async Task Azure_Preserves_Version_With_Only_Global_Arguments()
    {
        const string globalArguments = """
            Global Arguments
                --debug            : Increase logging verbosity to show all debug logs.
                --help -h          : Show this help message and exit.
                --only-show-errors : Only show errors, suppressing warnings.
                --output -o        : Output format. Allowed values: json, jsonc, none, table, tsv, yaml, yamlc.
                --query            : JMESPath query string.
                --subscription     : Name or ID of subscription.
                --verbose          : Increase logging verbosity.
            """;
        var executor = new StubExecutor(new Dictionary<string, string>
        {
            ["--help"] = "Group\n    az : Azure CLI.\n\nCommands:\n    version : Show versions.\n    clear : Clear state.\n\n" + globalArguments,
            ["version --help"] = "Command\n    az version : Show the versions of Azure CLI modules and extensions.\n\nArguments\n\n" + globalArguments,
            ["clear --help"] = "Command\n    az clear : Clear state.\n\nArguments\n\n" + globalArguments,
        });
        var scraper = new AzCliScraper(executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AzCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Select(command => command.FullCommand)).IsEquivalentTo(["az version"]);
        await Assert.That(commands.Single().ClassName).IsEqualTo("AzVersionOptions");
        await Assert.That(commands.Single().Options).IsEmpty();
        await Assert.That(executor.Arguments).Contains("clear --help");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Version_Subcommands_Are_Generated_With_Or_Without_Options(bool hasOptions)
    {
        var executor = new StubExecutor(new Dictionary<string, string>
        {
            ["--help"] = """
                Usage:
                  fake [command]

                Available Commands:
                  version     Print the tool version
                  help        Show help
                  completion  Generate shell completion

                Flags:
                  --version   Print the tool version
                  --help      Show help
                """,
            ["version --help"] = "Usage:\n  fake version\n" + (hasOptions
                ? "\nFlags:\n  --short   Print only the version number\n"
                : string.Empty),
        });
        var scraper = new TestCobraScraper(executor);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Select(command => command.FullCommand)).IsEquivalentTo(["fake version"]);
        await Assert.That(commands.Single().Options.Select(option => option.SwitchName))
            .IsEquivalentTo(hasOptions ? ["--short"] : Array.Empty<string>());
        await Assert.That(executor.Arguments).DoesNotContain("help --help");
        await Assert.That(executor.Arguments).DoesNotContain("completion --help");
    }
}
