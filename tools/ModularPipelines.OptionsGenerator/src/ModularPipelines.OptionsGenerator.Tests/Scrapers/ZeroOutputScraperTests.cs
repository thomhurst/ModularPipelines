using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class ZeroOutputScraperTests
{
    [Test]
    public async Task Aws_Extracts_Current_Rendered_Service_List()
    {
        const string helpText = """
            Available Services
            ******************

            * accessanalyzer

            * cloudformation

            * ec2
            """;

        await Assert.That(new TestAwsCliScraper().Extract(helpText))
            .IsEquivalentTo(["accessanalyzer", "cloudformation", "ec2"]);
    }

    [Test]
    public async Task Gh_Parses_Uppercase_Usage_And_Flags_Sections()
    {
        const string helpText = """
            Create an issue.

            USAGE
              gh issue create [flags]

            FLAGS
              -t, --title string   Supply a title
                  --web            Open the browser
            """;

        var command = await new TestGhCliScraper().Parse(["gh", "issue", "create"], helpText);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--title", "--web"]);
    }

    [Test]
    public async Task Yarn_Extracts_Classic_Bulleted_Command_List()
    {
        const string helpText = """
              Commands:
                - add
                - install
                - workspace
            """;

        await Assert.That(new TestYarnCliScraper().Extract(helpText))
            .IsEquivalentTo(["add", "install", "workspace"]);
    }

    private sealed class TestAwsCliScraper : AwsCliScraper
    {
        public TestAwsCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<AwsCliScraper>.Instance)
        {
        }

        public IReadOnlyList<string> Extract(string helpText) => ExtractSubcommands(helpText).ToList();
    }

    private sealed class TestGhCliScraper : GhCliScraper
    {
        public TestGhCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<GhCliScraper>.Instance)
        {
        }

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
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

        public IReadOnlyList<string> Extract(string helpText) => ExtractSubcommands(helpText).ToList();
    }
}
