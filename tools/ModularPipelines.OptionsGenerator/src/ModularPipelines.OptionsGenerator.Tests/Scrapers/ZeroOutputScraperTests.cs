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
    public async Task Gh_Parses_Inline_Usage_And_Indented_Flags_Sections()
    {
        const string helpText = """
            Create an issue.

            Usage: gh issue create [flags]

              Flags:
                -t, --title string   Supply a title
            """;

        var command = await new TestGhCliScraper().Parse(["gh", "issue", "create"], helpText);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--title"]);
    }

    [Test]
    public async Task Gh_Parses_Indented_Usage_Heading()
    {
        const string helpText = """
            Create an issue.

              Usage:
                gh issue create [flags]

            Flags:
              -t, --title string   Supply a title
            """;

        var command = await new TestGhCliScraper().Parse(["gh", "issue", "create"], helpText);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--title"]);
    }

    [Test]
    public async Task Gh_Extracts_General_And_Targeted_Child_Commands()
    {
        const string helpText = """
            Work with GitHub issues.

            USAGE
              gh issue <command> [flags]

            GENERAL COMMANDS
              create:        Create a new issue
              list:          List issues in a repository
              status:        Show status of relevant issues

            TARGETED COMMANDS
              close:         Close issue
              view:          View an issue

            FLAGS
              -R, --repo [HOST/]OWNER/REPO   Select another repository
            """;

        await Assert.That(new TestGhCliScraper().Extract(helpText))
            .IsEquivalentTo(["create", "list", "status", "close", "view"]);
    }

    [Test]
    public async Task Gh_Extracts_Core_Pr_Child_Commands()
    {
        const string helpText = """
            Work with GitHub pull requests.

            USAGE
              gh pr <command> [flags]

            GENERAL COMMANDS
              create:        Create a pull request
              list:          List pull requests

            TARGETED COMMANDS
              checkout:      Check out a pull request
              merge:         Merge a pull request
              view:          View a pull request
            """;

        await Assert.That(new TestGhCliScraper().Extract(helpText))
            .IsEquivalentTo(["create", "list", "checkout", "merge", "view"]);
    }

    [Test]
    public async Task Gh_Api_Models_Field_And_Header_Options_As_Repeatable()
    {
        const string helpText = """
            Makes an authenticated HTTP request.

            USAGE
              gh api <endpoint> [flags]

            FLAGS
              -F, --field key=value       Add a typed parameter
              -H, --header key:value      Add a HTTP request header
              -f, --raw-field key=value   Add a string parameter
            """;

        var command = await new TestGhCliScraper().Parse(["gh", "api"], helpText);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.Options.All(option => option.AcceptsMultipleValues)).IsTrue();
        await Assert.That(command.Options.Select(option => option.CSharpType))
            .IsEquivalentTo(Enumerable.Repeat("IEnumerable<string>?", 3));
    }

    [Test]
    public async Task Gh_Search_Models_Explicit_Boolean_Filter_As_Value_Taking()
    {
        const string helpText = """
            Search for issues on GitHub.

            USAGE
              gh search issues [<query>] [flags]

            FLAGS
                  --archived   Filter based on the repository archived state {true|false}
            """;

        var command = await new TestGhCliScraper().Parse(["gh", "search", "issues"], helpText);
        var archived = command!.Options.Single();

        await Assert.That(archived.IsFlag).IsFalse();
        await Assert.That(archived.CSharpType).IsEqualTo("bool?");
        await Assert.That(archived.ValueSeparator).IsEqualTo("=");
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

        public IReadOnlyList<string> Extract(string helpText) => ExtractSubcommands(helpText).ToList();
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
