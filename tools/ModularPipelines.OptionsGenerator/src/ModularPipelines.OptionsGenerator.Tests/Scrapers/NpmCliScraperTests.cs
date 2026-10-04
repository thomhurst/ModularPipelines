using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class NpmCliScraperTests
{
    private static readonly string[] OrgVerbs = ["set", "rm", "ls"];

    [Test]
    public async Task Discovers_Wrapped_Root_Commands()
    {
        var scraper = CreateScraper();

        await Assert.That(scraper.GetSubcommands("""
            npm <command>

            All commands:

                access, adduser, audit, bugs, cache, ci, completion,
                config, dedupe, dist-tag, install, package-name

            Specify configs in the ini-formatted file:
            """))
            .IsEquivalentTo([
                "access",
                "adduser",
                "audit",
                "bugs",
                "cache",
                "ci",
                "completion",
                "config",
                "dedupe",
                "dist-tag",
                "install",
                "package-name",
            ]);
    }

    [Test]
    public async Task Parses_Npm_Options_And_Positionals()
    {
        var scraper = CreateScraper();
        var command = await scraper.Parse(
            ["npm", "install"],
            """
            Install a package

            Usage:
            npm install [<package-spec> ...]

            Options:
            [-S|--save] [-g|--global] [--omit <type> [--omit <type> ...]]

              -S|--save
                Save installed packages.

              -g|--global
                Operates in global mode.

              --omit
                Dependency types to omit. This option may be specified multiple times.
            """);

        await Assert.That(command!.ClassName).IsEqualTo("NpmInstallOptions");
        await Assert.That(command.PositionalArguments.Single().CSharpType)
            .IsEqualTo("IEnumerable<string>?");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--save").ShortForm)
            .IsEqualTo("-S");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--global").IsFlag)
            .IsTrue();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--omit").CSharpType)
            .IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    public async Task Keeps_Synopsis_Explanation_Out_Of_Command_Parts()
    {
        var scraper = CreateScraper();
        var command = await scraper.Parse(
            ["npm", "init"],
            """
            Create a package.json file

            Usage:
            npm init <package-spec> (same as `npx create-<package-spec>`)
            npm init <@scope> (same as `npx <@scope>/create`)
            """);

        await Assert.That(command!.CommandParts).IsEquivalentTo(["init"]);
        await Assert.That(command.PositionalArguments).Count().IsEqualTo(1);
        await Assert.That(command.PositionalArguments[0].PropertyName).IsEqualTo("Value");
        await Assert.That(command.PositionalArguments[0].CSharpType).IsEqualTo("string?");
        await Assert.That(command.PositionalArguments[0].IsRequired).IsFalse();
    }

    [Test]
    public async Task Search_Does_Not_Treat_The_Operand_As_A_Subcommand()
    {
        var scraper = CreateScraper();
        var command = await scraper.Parse(
            ["npm", "search"],
            """
            Search for packages

            Usage:
            npm search <search term> [<search term> ...]
            """);

        await Assert.That(command!.CommandParts).IsEquivalentTo(["search"]);
        await Assert.That(command.PositionalArguments).Count().IsEqualTo(1);
    }

    [Test]
    public async Task Exec_Attaches_The_Separator_To_The_Command_Operand()
    {
        var scraper = CreateScraper();
        var command = await scraper.Parse(
            ["npm", "exec"],
            """
            Run a command

            Usage:
            npm exec --package=<pkg> -- <cmd> [args...]

            Options:
            [--package <package-spec>]
            """);

        await Assert.That(command!.CommandParts).IsEquivalentTo(["exec"]);
        var operand = command.PositionalArguments[0];
        await Assert.That(operand.Phase).IsEqualTo(CommandLinePhase.Passthrough);
        await Assert.That(operand.PrependOptionTerminator).IsTrue();
    }

    [Test]
    public async Task Discovers_Nested_Commands_From_Usage_Without_Operands_Or_Assignments()
    {
        const string help = """
            Set access level on published packages
            Usage:
            npm access list packages [<user>|<scope>]
            npm access list collaborators [<package>]
            npm access set status=public|private [<package>]
            npm access grant <read-only|read-write> <scope:team>
            """;
        var scraper = CreateScraper();
        await Assert.That(scraper.GetSubcommands(["npm", "access"], help)).IsEquivalentTo(["list", "set", "grant"]);
        await Assert.That(scraper.GetSubcommands(["npm", "access", "list"], help)).IsEquivalentTo(["packages", "collaborators"]);
        await Assert.That(scraper.GetSubcommands(["npm", "access", "set"], help)).IsEmpty();
        await Assert.That(scraper.GetSubcommands(["npm", "access", "grant"], help)).IsEmpty();
    }

    [Test]
    public async Task Command_Groups_Do_Not_Become_Executable_Options()
    {
        var command = await CreateScraper().Parse(["npm", "token"], """
            Manage tokens
            Usage:
            npm token list
            npm token revoke <id>
            Options:
            [--json]
              --json
                Output JSON.
            """);
        await Assert.That(command).IsNull();
    }

    [Test]
    public async Task Summary_Alternatives_Preserve_Boolean_Options_And_Negation()
    {
        var command = await CreateScraper().Parse(["npm", "install"], """
            Install packages
            Usage:
            npm install [<package-spec> ...]
            Options:
            [-S|--save|--no-save|--save-prod|--save-dev] [--no-package-lock]

              -S|--save
                Save installed packages.

              --package-lock
                Read the lockfile.
            """);
        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--save", "--save-prod", "--save-dev", "--package-lock"]);
        await Assert.That(command.Options.Single(option => option.SwitchName == "--save").NegatedSwitchName).IsEqualTo("--no-save");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--package-lock").NegatedSwitchName).IsEqualTo("--no-package-lock");
    }

    [Test]
    public async Task Root_Verb_Choices_Are_Discovered_But_Nested_Value_Choices_Are_Not()
    {
        var scraper = CreateScraper();
        const string auditHelp = "Run an audit\nUsage:\nnpm audit [fix|signatures]\nOptions:\n[--json]";
        await Assert.That(scraper.GetSubcommands(["npm", "audit"], auditHelp)).IsEquivalentTo(["fix", "signatures"]);
        var fix = await scraper.Parse(["npm", "audit", "fix"], auditHelp);
        await Assert.That(fix!.CommandParts).IsEquivalentTo(["audit", "fix"]);
        await Assert.That(fix.PositionalArguments).IsEmpty();
        await Assert.That(scraper.GetSubcommands(["npm", "profile", "enable-2fa"],
            "Usage:\nnpm profile enable-2fa [auth-only|auth-and-writes]")).IsEmpty();
    }

    [Test]
    public async Task Org_Bare_Operand_Names_Are_Not_Literal_Subcommands()
    {
        var help = await ReadNpmFixture("org");
        var scraper = CreateScraper();
        await Assert.That(scraper.GetSubcommands(["npm", "org"], help)).IsEquivalentTo(["set", "rm", "ls"]);
        foreach (var verb in OrgVerbs)
        {
            await Assert.That(scraper.GetSubcommands(["npm", "org", verb], help)).IsEmpty();
            var command = await scraper.Parse(["npm", "org", verb], help);
            await Assert.That(command).IsNotNull();
            await Assert.That(command!.PositionalArguments[0].PropertyName).IsEqualTo("Orgname");
            await Assert.That(command.PositionalArguments[0].IsRequired).IsTrue();
            await Assert.That(command.PositionalArguments[1].PropertyName).IsEqualTo("Username");
            await Assert.That(command.PositionalArguments[1].IsRequired).IsEqualTo(verb != "ls");
        }
    }

    [Test]
    [Arguments("get")]
    [Arguments("set")]
    public async Task Config_Alias_Explanations_Are_Not_Operands(string verb)
    {
        var command = await CreateScraper().Parse(["npm", verb], await ReadNpmFixture(verb));
        await Assert.That(command!.PositionalArguments).Count().IsEqualTo(1);
        await Assert.That(command.PositionalArguments[0].IsVariadic).IsTrue();
        await Assert.That(command.PositionalArguments[0].IsRequired).IsEqualTo(verb == "set");
    }

    [Test]
    public async Task Bare_Option_Value_Hints_Are_Not_Flags()
    {
        var command = await CreateScraper().Parse(["npm", "version"], await ReadNpmFixture("version"));
        var preid = command!.Options.Single(option => option.SwitchName == "--preid");
        await Assert.That(preid.IsFlag).IsFalse();
        await Assert.That(preid.CSharpType).IsEqualTo("string?");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--json").IsFlag).IsTrue();
    }

    private static Task<string> ReadNpmFixture(string command) => File.ReadAllTextAsync(
        Path.Combine(AppContext.BaseDirectory, "Fixtures", "Npm", "11.11.0", $"npm-{command}.txt"));

    private static TestNpmCliScraper CreateScraper() => new();

    private sealed class TestNpmCliScraper : NpmCliScraper
    {
        public TestNpmCliScraper()
            : base(
                new UnusedExecutor(),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<NpmCliScraper>.Instance)
        {
        }

        public string[] GetSubcommands(string helpText) => [.. ExtractSubcommands(helpText)];

        public string[] GetSubcommands(string[] path, string helpText) => [.. ExtractSubcommands(path, helpText)];

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }

    private sealed class UnusedExecutor : ICliCommandExecutor
    {
        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            throw new NotSupportedException();
    }
}
