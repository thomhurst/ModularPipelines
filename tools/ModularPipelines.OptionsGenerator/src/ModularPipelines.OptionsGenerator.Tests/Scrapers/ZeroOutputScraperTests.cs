using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
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
    public async Task Aws_Extracts_Service_List_With_Ansi_Formatting()
    {
        const string helpText = "\u001b[4mAVAILABLE SERVICES\u001b[24m\n\n"
            + "       \u001b[1mo \u001b]8;;https://example.test\u001b\\accessanalyzer\u001b]8;;\u001b\\\u001b[22m\n"
            + "       \u001b[1mo cloudformation\u001b[22m\n"
            + "       \u001b[1mo ec2\u001b[22m\n";
        const string commandHelp = "OPTIONS\n       --enabled (boolean)\n";
        var scraper = new TestAwsCliScraper(new StubExecutor(arguments =>
            arguments == "help" ? helpText : commandHelp));

        await Assert.That((await ScrapeAsync(scraper)).Select(command => command.FullCommand))
            .IsEquivalentTo(["aws accessanalyzer", "aws cloudformation", "aws ec2"]);
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
    [Arguments("create")]
    [Arguments("edit")]
    public async Task Gh_Release_Latest_Preserves_Explicit_False(string commandName)
    {
        const string helpText = """
            USAGE
              gh release COMMAND [flags]

            FLAGS
                  --latest   Mark this release as Latest (default: automatic based on date)
            """;

        var command = await new TestGhCliScraper().Parse(
            ["gh", "release", commandName],
            helpText.Replace("COMMAND", commandName, StringComparison.Ordinal));
        var latest = command!.Options.Single(option => option.PropertyName == "Latest");

        using (Assert.Multiple())
        {
            await Assert.That(latest.IsFlag).IsFalse();
            await Assert.That(latest.CSharpType).IsEqualTo("bool?");
        }
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
    public async Task Gh_Command_Groups_Drop_Command_Placeholders()
    {
        const string helpText = """
            Work with GitHub issues.

            USAGE
              gh issue <command> [flags]

            FLAGS
              -R, --repo string   Select another repository
            """;

        var command = await new TestGhCliScraper().Parse(["gh", "issue"], helpText);

        await Assert.That(command!.PositionalArguments).IsEmpty();
    }

    [Test]
    public async Task Gh_Codespace_Ssh_Keeps_Optional_Forwarded_Operands()
    {
        const string helpText = """
            SSH into a codespace.

            USAGE
              gh codespace ssh [<flags>...] [-- <ssh-flags>...] [<command>]

            FLAGS
              -c, --codespace string   Name of the codespace
            """;

        var command = await new TestGhCliScraper().Parse(["gh", "codespace", "ssh"], helpText);
        var arguments = command!.PositionalArguments.ToDictionary(argument => argument.PropertyName);

        using (Assert.Multiple())
        {
            await Assert.That(arguments.Keys).IsEquivalentTo(["Flags", "SshFlags", "Command"]);
            await Assert.That(arguments["SshFlags"].CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(arguments["SshFlags"].IsRequired).IsFalse();
            await Assert.That(arguments["Command"].CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Gh_Codespace_Cp_Models_Terminated_Scp_Flags_Before_Operands()
    {
        const string helpText = """
            Copy files to and from a codespace.

            USAGE
              gh codespace cp [-e] [-r] [-- [<scp flags>...]] <sources>... <dest>

            FLAGS
              -e, --expand      Expand remote file names
              -r, --recursive   Recursively copy directories
            """;

        var command = await new TestGhCliScraper().Parse(["gh", "codespace", "cp"], helpText);
        var arguments = command!.PositionalArguments.ToDictionary(argument => argument.PropertyName);

        using (Assert.Multiple())
        {
            await Assert.That(arguments.Keys).IsEquivalentTo(["ScpFlags", "Sources", "Dest"]);
            await Assert.That(arguments["ScpFlags"].PrependOptionTerminator).IsTrue();
            await Assert.That(arguments["ScpFlags"].Phase).IsEqualTo(CommandLinePhase.Passthrough);
            await Assert.That(arguments["Sources"].Phase).IsEqualTo(CommandLinePhase.LateOperand);
            await Assert.That(arguments["Dest"].Phase).IsEqualTo(CommandLinePhase.LateOperand);
        }

        var tool = new CliToolDefinition
        {
            ToolName = "gh",
            NamespacePrefix = "Gh",
            TargetNamespace = "ModularPipelines.GitHub",
            OutputDirectory = "src/ModularPipelines.GitHub",
            Commands = [command],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(
            InheritedPropertyCollisionResolver.Resolve(tool))).Single().Content;
        await Assert.That(generated).Contains("IEnumerable<string>? ScpFlags");
    }

    [Test]
    public async Task Gh_Issue_Edit_Models_Number_Or_Url_Targets_As_A_Collection()
    {
        const string helpText = """
            Edit issues.

            USAGE
              gh issue edit {<numbers> | <urls>} [flags]

            FLAGS
              -t, --title string   Set the new title
            """;

        var command = await new TestGhCliScraper().Parse(["gh", "issue", "edit"], helpText);
        var argument = command!.PositionalArguments.Single();

        using (Assert.Multiple())
        {
            await Assert.That(argument.PropertyName).IsEqualTo("NumbersOrUrls");
            await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>");
            await Assert.That(argument.IsRequired).IsTrue();
            await Assert.That(argument.IsVariadic).IsTrue();
        }
    }

    [Test]
    public async Task Gh_Pr_Alternatives_Use_Accurate_Property_Name()
    {
        var command = await new TestGhCliScraper().Parse(
            ["gh", "pr", "view"],
            "USAGE\n  gh pr view [<number> | <url> | <branch>] [flags]\n\nFLAGS\n  --web   Open in browser");

        var argument = command!.PositionalArguments.Single();
        await Assert.That(argument.PropertyName).IsEqualTo("NumberOrUrlOrBranch");
        await Assert.That(argument.CSharpType).IsEqualTo("string?");
    }

    [Test]
    public async Task Gh_Secret_Set_Masks_Body_But_Not_Name()
    {
        const string helpText = """
            Set a secret value.

            USAGE
              gh secret set <secret-name> [flags]

            FLAGS
              -b, --body string   The value for the secret (reads from standard input if not specified)
            """;

        var command = await new TestGhCliScraper().Parse(["gh", "secret", "set"], helpText);
        var secretName = command!.PositionalArguments.Single();
        var body = command.Options.Single(option => option.PropertyName == "Body");

        await Assert.That(secretName.PropertyName).IsEqualTo("SecretName");
        await Assert.That(secretName.IsSecret).IsFalse();
        await Assert.That(body.IsSecret).IsTrue();
    }

    [Test]
    public async Task Gh_Secret_Delete_Does_Not_Mask_Name()
    {
        const string helpText = """
            Delete a secret.

            USAGE
              gh secret delete <secret-name> [flags]
            """;

        var command = await new TestGhCliScraper().Parse(["gh", "secret", "delete"], helpText);

        await Assert.That(command!.PositionalArguments.Single().IsSecret).IsFalse();
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

    [Test]
    public async Task Yarn_Extracts_Berry_Commands_With_Ansi_Formatting()
    {
        const string helpText = "\u001b[1m━━━ General commands ━━━━━━━━━━━\u001b[0m\n\n"
            + "  \u001b[1myarn \u001b]8;;https://example.test\u001b\\add\u001b]8;;\u001b\\ [--json]\u001b[22m\n"
            + "    add dependencies to the project\n\n"
            + "  \u001b[1myarn cache clean [--mirror]\u001b[22m\n"
            + "    remove the shared cache files\n\n"
            + "\u001b[1m━━━ Npm-related commands ━━━━━━━\u001b[0m\n\n"
            + "  \u001b[1myarn npm info [--json]\u001b[22m\n";
        const string commandHelp = "━━━ Details ━━━\nCommand details.\n\n"
            + "━━━ Options ━━━\n  --json  Format output as JSON.\n";
        var scraper = new TestYarnCliScraper(new StubExecutor(arguments => arguments switch
        {
            "--help" => helpText,
            _ => commandHelp,
        }));

        await Assert.That((await ScrapeAsync(scraper)).Select(command => command.FullCommand))
            .IsEquivalentTo(["yarn add", "yarn cache clean", "yarn npm info"]);
    }

    private static async Task<IReadOnlyList<CliCommandDefinition>> ScrapeAsync(ICliScraper scraper)
    {
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        return commands;
    }

    private sealed class TestAwsCliScraper : AwsCliScraper
    {
        public TestAwsCliScraper(ICliCommandExecutor? executor = null)
            : base(
                executor ?? new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
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

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }

        public IReadOnlyList<string> Extract(string helpText) => ExtractSubcommands(helpText).ToList();
    }

    private sealed class TestYarnCliScraper : YarnCliScraper
    {
        public TestYarnCliScraper(ICliCommandExecutor? executor = null)
            : base(
                executor ?? new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<YarnCliScraper>.Instance)
        {
        }

        public IReadOnlyList<string> Extract(string helpText) => ExtractSubcommands(helpText).ToList();

        protected override async Task<string?> GetHelpTextAsync(
            string[] commandPath,
            CancellationToken cancellationToken)
        {
            var arguments = commandPath.Length > 1
                ? string.Join(" ", commandPath.Skip(1)) + " --help"
                : "--help";
            var result = await Executor.ExecuteAsync(ToolName, arguments, cancellationToken);
            return result.StandardOutput;
        }
    }

    private sealed class StubExecutor(Func<string, string> outputByArguments) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = outputByArguments(arguments),
                StandardError = string.Empty,
            });

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
