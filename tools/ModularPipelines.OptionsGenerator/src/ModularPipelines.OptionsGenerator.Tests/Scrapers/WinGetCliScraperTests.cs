using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class WinGetCliScraperTests
{
    [Test]
    public async Task Extracts_Subcommands_From_Hyphenated_Heading()
    {
        const string helpText = """
            usage: winget source [<command>] [<options>]

            The following sub-commands are available:
              add     Add a new source
              list    List current sources

            For more details on a specific command, pass it the help argument. [-?]
            """;

        var commands = new TestWinGetCliScraper().Extract(helpText);

        await Assert.That(commands).IsEquivalentTo(["add", "list"]);
    }

    [Test]
    public async Task Command_Group_Does_Not_Model_Child_Command_Placeholder()
    {
        const string helpText = """
            Manage package sources.

            usage: winget source [<command>] [<options>]

            The following sub-commands are available:
              add     Add a new source
              list    List current sources
            """;

        var command = await new TestWinGetCliScraper().Parse(["winget", "source"], helpText);

        await Assert.That(command!.PositionalArguments).IsEmpty();
        await Assert.That(command.UsagePositionalArguments).IsEmpty();
    }

    [Test]
    public async Task Models_Repeatable_Sort_As_A_Collection()
    {
        const string helpText = """
            List installed packages.

            usage: winget list [<options>]

            The following options are available:
              --sort   Sort results by a property (can be repeated)
            """;

        var command = await new TestWinGetCliScraper().Parse(
            ["winget", "list"],
            helpText);
        var sort = command!.Options.Single(option => option.SwitchName == "--sort");

        using (Assert.Multiple())
        {
            await Assert.That(sort.AcceptsMultipleValues).IsTrue();
            await Assert.That(sort.CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    public async Task Explicit_Repeatability_Takes_Precedence_Over_Boolean_Heuristics()
    {
        const string helpText = """
            List installed packages.

            usage: winget list [<options>]

            The following options are available:
              --source   Accepts multiple values
            """;

        var command = await new TestWinGetCliScraper().Parse(
            ["winget", "list"],
            helpText);
        var source = command!.Options.Single(option => option.SwitchName == "--source");

        using (Assert.Multiple())
        {
            await Assert.That(source.IsFlag).IsFalse();
            await Assert.That(source.AcceptsMultipleValues).IsTrue();
            await Assert.That(source.CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    public async Task Does_Not_Mark_Boolean_Flags_As_Repeatable_Values()
    {
        const string helpText = """
            List installed packages.

            usage: winget list [<options>]

            The following options are available:
              --verbose   Enable verbose logging multiple times during troubleshooting
            """;

        var command = await new TestWinGetCliScraper().Parse(
            ["winget", "list"],
            helpText);
        var verbose = command!.Options.Single(option => option.SwitchName == "--verbose");

        using (Assert.Multiple())
        {
            await Assert.That(verbose.IsFlag).IsTrue();
            await Assert.That(verbose.AcceptsMultipleValues).IsFalse();
            await Assert.That(verbose.CSharpType).IsEqualTo("bool?");
        }
    }

    [Test]
    public async Task Authentication_Mode_With_Explicit_Choices_Is_A_Value_Option()
    {
        const string helpText = """
            Add a package pin.

            usage: winget pin add [<options>]

            The following options are available:
              --authentication-mode   Specify authentication window preference (silent, silentPreferred, or interactive)
            """;

        var command = await new TestWinGetCliScraper().Parse(
            ["winget", "pin", "add"],
            helpText);
        var authenticationMode = command!.Options.Single(option =>
            option.SwitchName == "--authentication-mode");

        using (Assert.Multiple())
        {
            await Assert.That(authenticationMode.IsFlag).IsFalse();
            await Assert.That(authenticationMode.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Traversal_Keeps_Boolean_Flags_With_Repeatability_Wording()
    {
        const string rootHelp = """
            Windows Package Manager

            usage: winget [<command>]

            The following commands are available:
              list   List installed packages
            """;
        const string listHelp = """
            Windows Package Manager

            usage: winget list [<options>]

            The following options are available:
              --verbose   Enable verbose logging multiple times during troubleshooting
            """;
        var scraper = new TestWinGetCliScraper(new StubExecutor(rootHelp, listHelp));
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var verbose = commands.Single().Options.Single(option => option.SwitchName == "--verbose");
        using (Assert.Multiple())
        {
            await Assert.That(verbose.IsFlag).IsTrue();
            await Assert.That(verbose.AcceptsMultipleValues).IsFalse();
        }
    }

    [Test]
    public async Task Search_Query_Usage_Is_Covered_By_Named_Option()
    {
        const string helpText = """
            Searches for packages from configured sources.

            usage: winget search [[-q] <query>] [<options>]

            The following arguments are available:
              -q,--query   The query used to search for a package
            """;
        var scraper = new TestWinGetCliScraper();
        var command = await scraper.Parse(["winget", "search"], helpText);
        var usage = scraper.ParseUsage(["winget", "search"], helpText);

        command!.ValidateOperandCoverage();

        using (Assert.Multiple())
        {
            await Assert.That(command.Options.Single().PropertyName).IsEqualTo("Query");
            await Assert.That(command.PositionalArguments).IsEmpty();
            await Assert.That(command.UsagePositionalArguments).HasSingleItem();
            await Assert.That(usage.PositionalArguments.Single().AssociatedOptionSwitch)
                .IsEqualTo("-q");
        }
    }

    [Test]
    public async Task Named_Argument_Usage_Remains_A_Named_Option()
    {
        const string helpText = """
            Writes installed packages to a file.

            usage: winget export [-o] <output> [<options>]

            The following arguments are available:
              -o,--output   File where the result is to be written
            """;
        var command = await new TestWinGetCliScraper().Parse(["winget", "export"], helpText);

        command!.ValidateOperandCoverage();

        using (Assert.Multiple())
        {
            await Assert.That(command.Options.Single().PropertyName).IsEqualTo("Output");
            await Assert.That(command.Options.Single().IsRequired).IsFalse();
            await Assert.That(command.PositionalArguments).IsEmpty();
        }
    }

    [Test]
    public async Task Skips_Unicode_Copyright_Banner_When_Extracting_Description()
    {
        const string helpText = """
            Windows Package Manager v1.29.290
            © 2026 Microsoft. All rights reserved.

            Add a new package pin.

            usage: winget pin add [<options>]
            """;

        var command = await new TestWinGetCliScraper().Parse(["winget", "pin", "add"], helpText);

        await Assert.That(command!.Description).IsEqualTo("Add a new package pin.");
    }

    [Test]
    public async Task Models_Known_No_Value_Options_As_Flags()
    {
        const string helpText = """
            Add a package pin.

            usage: winget pin add [<options>]

            The following options are available:
              -e,--exact     Find package using exact match
              --force        Direct run the command and continue with non security related issues
              --blocking     Block from upgrading until the pin is removed
              --installed    Pin a specific installed version
              --wait         Prompts the user to press any key before exiting
            """;

        var command = await new TestWinGetCliScraper().Parse(["winget", "pin", "add"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--exact", "--force", "--blocking", "--installed", "--wait"]);
            await Assert.That(command.Options.All(option => option.IsFlag)).IsTrue();
            await Assert.That(command.Options.All(option => option.CSharpType == "bool?")).IsTrue();
        }
    }

    [Test]
    public async Task Source_Add_Explicit_Is_A_Flag()
    {
        const string helpText = """
            Add a source.

            usage: winget source add [<options>]

            The following options are available:
              --explicit   Excludes a source from discovery unless specified
            """;

        var command = await new TestWinGetCliScraper().Parse(["winget", "source", "add"], helpText);
        var explicitOption = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(explicitOption.IsFlag).IsTrue();
            await Assert.That(explicitOption.CSharpType).IsEqualTo("bool?");
        }
    }

    [Test]
    public async Task Source_Edit_Explicit_Remains_A_Valued_Option()
    {
        const string helpText = """
            Edit a source.

            usage: winget source edit [<options>]

            The following options are available:
              --explicit   Determines whether the source is explicit. Valid values: true, false
            """;

        var command = await new TestWinGetCliScraper().Parse(["winget", "source", "edit"], helpText);
        var explicitOption = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(explicitOption.IsFlag).IsFalse();
            await Assert.That(explicitOption.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Uses_Serial_Help_Traversal()
    {
        await Assert.That(new TestWinGetCliScraper().Parallelism).IsEqualTo(1);
    }

    [Test]
    public async Task Wrapped_Descriptions_That_Look_Like_Option_Rows_Stay_Prose()
    {
        // The wrapped "--exact  to ..." line deliberately keeps two spaces so it satisfies the
        // option-row pattern; only its column keeps it inside the description.
        const string helpText = """
            usage: winget install [[-q] <query>] [<options>]

            The following options are available:
              -q,--query                    The query used to search for a package. Combine with
                                            --exact  to match the query exactly.
              -e,--exact                    Find package using exact match
            """;

        var command = await new TestWinGetCliScraper().Parse(["winget", "install"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--query", "--exact"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--query").Description)
                .IsEqualTo("The query used to search for a package. Combine with --exact  to match the query exactly.");
        }
    }

    private sealed class TestWinGetCliScraper : WinGetCliScraper
    {
        public TestWinGetCliScraper(ICliCommandExecutor? executor = null)
            : base(
                executor ?? new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<WinGetCliScraper>.Instance)
        {
        }

        public override Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public IReadOnlyList<string> Extract(string helpText) =>
            ExtractSubcommands(helpText).ToList();

        public async Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            var command = await ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
            if (command is not null)
            {
                usage = NormalizeUsageSynopsis(command, usage);
            }

            return command is null
                ? null
                : command with { UsagePositionalArguments = usage.PositionalArguments };
        }

        public UsageSynopsisParseResult ParseUsage(string[] commandPath, string helpText) =>
            ParseUsageSynopsis(commandPath, helpText);

        public int Parallelism => MaxParallelism;
    }

    private sealed class StubExecutor(string rootHelp, string listHelp) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = arguments == "--help" ? rootHelp : listHelp,
                StandardError = string.Empty,
            });

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
