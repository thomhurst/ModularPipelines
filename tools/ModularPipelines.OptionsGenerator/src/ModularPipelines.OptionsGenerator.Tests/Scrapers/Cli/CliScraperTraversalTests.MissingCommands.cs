using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class CliScraperTraversalTests
{
    [Test]
    public async Task CobraTraversal_Discovers_Qualified_Command_Sections()
    {
        var scraper = new TestCobraScraper(new StubExecutor(new Dictionary<string, string>()));
        var commands = scraper.GetSubcommands("""
            Basic Commands (Beginner):
              create  Create a resource
              run     Run an image
            Basic Commands (Intermediate):
              get     Display resources
              delete  Delete resources
            Deploy Commands:
              rollout Manage a rollout
            """);

        await Assert.That(commands).IsEquivalentTo(new[] { "create", "run", "get", "delete", "rollout" });
    }

    [Test]
    public async Task SharedTraversal_Preserves_Leaf_Without_Options_Or_Operands()
    {
        var scraper = new TestCobraScraper(new StubExecutor(new Dictionary<string, string>
        {
            ["--help"] = """
                Usage:
                  fake [command]

                Available Commands:
                  config  Manage configuration
                """,
            ["config --help"] = """
                Available Commands:
                  current-context  Display the current context
                """,
            ["config current-context --help"] = """
                Display the current context.

                Usage:
                  fake config current-context
                """,
        }));

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(new[] { "fake config current-context" });
        await Assert.That(commands.Single().Options).IsEmpty();
        await Assert.That(commands.Single().PositionalArguments).IsEmpty();
    }

    [Test]
    [Arguments("[Preview]")]
    [Arguments("[Experimental]")]
    [Arguments("[Deprecated]")]
    public async Task AzureTraversal_Discovers_Annotated_Storage_Commands(string annotation)
    {
        var scraper = new AzCliScraper(
            new StubExecutor(new Dictionary<string, string>
            {
                ["--help"] = """
                    Group
                        az : Manage Azure resources.
                    Subgroups:
                        storage : Manage storage.
                    """,
                ["storage --help"] = $"""
                    Group
                        az storage : Manage storage.
                    Subgroups:
                        queue   {annotation} : Manage storage queues.
                    """,
                ["storage queue --help"] = $"""
                    Group
                        az storage queue : Manage storage queues.
                    Commands:
                        create {annotation} : Create a queue under the given account.
                    """,
                ["storage queue create --help"] = """
                    Command
                        az storage queue create : Create a queue under the given account.
                    Arguments
                        --name -n [Required] : The queue name.
                    """,
            }),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AzCliScraper>.Instance);

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(new[] { "az storage queue create" });
    }

    [Test]
    public async Task AzureTraversal_Ignores_Command_Header_Text_Within_Prose()
    {
        var scraper = new AzCliScraper(
            new StubExecutor(new Dictionary<string, string>
            {
                ["--help"] = """
                    Group
                        az : Manage Azure resources.
                    Subgroups:
                        storage : Manage storage.
                    """,
                ["storage --help"] = """
                    Group
                        az storage : Manage storage.
                    Subgroups:
                        blob : Manage blobs.
                    """,
                ["storage blob --help"] = """
                    Group
                        az storage blob : Manage object storage.
                            Please specify authentication parameters for your commands: --auth-mode,
                            --account-key, --connection-string, --sas-token.
                    Subgroups:
                        metadata : Manage metadata.
                    Commands:
                        upload : Upload a blob.
                    """,
                ["storage blob metadata --help"] = """
                    Group
                        az storage blob metadata : Manage metadata.
                    Commands:
                        show : Show metadata.
                    """,
                ["storage blob metadata show --help"] = """
                    Command
                        az storage blob metadata show : Show metadata.
                    Arguments
                        --name : Blob name.
                    """,
                ["storage blob upload --help"] = """
                    Command
                        az storage blob upload : Upload a blob.
                    Arguments
                        --file : Path to the file.
                    """,
            }),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AzCliScraper>.Instance);

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands.Select(command => command.FullCommand)).IsEquivalentTo(new[]
        {
            "az storage blob metadata show", "az storage blob upload",
        });
    }
}
