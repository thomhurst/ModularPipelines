using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
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

              yarn cache clean
                remove cached archives

              yarn rebuild ...
                rebuild native packages

              yarn node …
                run Node with Yarn's module resolution
            """;

        var commands = new TestYarnCliScraper().Extract(helpText);

        await Assert.That(commands).IsEquivalentTo(
            ["add", "bin", "workspace", "workspaces focus", "cache clean", "rebuild", "node"]);
    }

    [Test]
    public async Task Parses_Clipanion_Usage_Operands_And_Numbered_Option_Values()
    {
        const string helpText = """
            ━━━ Usage ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

              $ yarn dlx [-p,--package #0] [-q,--quiet] <command>

            ━━━ Options ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

              -p,--package #0    The package to install before running the command
              -q,--quiet         Only report critical errors
            """;

        var command = await new TestYarnCliScraper().Parse(
            ["yarn", "dlx"],
            helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.PositionalArguments.Single().PropertyName)
                .IsEqualTo("Command");
            await Assert.That(command.PositionalArguments.Single().IsRequired).IsTrue();
            await Assert.That(command.Options.Single(option => option.PropertyName == "Package").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.PropertyName == "Quiet").IsFlag)
                .IsTrue();
        }
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

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                UsageSynopsisParser.Parse(helpText, commandPath),
                CancellationToken.None);
    }
}
