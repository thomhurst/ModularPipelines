using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class YarnCliScraperTests
{
    [Test]
    public async Task Extracts_Current_Plain_Category_Commands()
    {
        const string helpText = """
            Yarn Package Manager - 4.17.1

              $ yarn <command>

            General commands

              yarn add [--json] [-F,--fixed] ...
                add dependencies to the project

              yarn bin [-v,--verbose] [--json] [name]
                get the path to a binary script

            Workspace-related commands

              yarn workspace <workspaceName> <commandName> ...
                run a command within the specified workspace

              yarn workspaces focus [--json] [-A,--all] ...
                install a single workspace and its dependencies
            """;

        var commands = new TestYarnCliScraper().Extract(helpText);

        await Assert.That(commands).IsEquivalentTo(
            ["add", "bin", "workspace", "workspaces", "workspaces focus"]);
    }

    private sealed class TestYarnCliScraper : YarnCliScraper
    {
        public TestYarnCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<YarnCliScraper>.Instance)
        {
        }

        public List<string> Extract(string helpText) => ExtractSubcommands(helpText).ToList();
    }
}
