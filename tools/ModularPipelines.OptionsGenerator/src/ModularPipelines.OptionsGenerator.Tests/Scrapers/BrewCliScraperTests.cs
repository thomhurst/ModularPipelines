using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class BrewCliScraperTests
{
    [Test]
    public async Task Traversal_Uses_Complete_Quiet_Command_Inventory()
    {
        var scraper = new TestBrewCliScraper(new CommandInventoryExecutor());
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Select(static command => command.FullCommand))
            .IsEquivalentTo(
            [
                "brew alpha",
                "brew baz.qux",
                "brew beta",
                "brew foo+bar",
                "brew update",
            ]);
    }

    [Test]
    public async Task Preserves_Positional_Operands_From_Usage()
    {
        const string helpText = """
            Usage: brew install [options] formula|cask [...]

            Install a formula or cask.

                  --formula   Treat all named arguments as formulae.
            """;

        var command = await new TestBrewCliScraper().Parse(
            ["brew", "install"],
            helpText);

        await Assert.That(command!.PositionalArguments).HasSingleItem();
        var argument = command.PositionalArguments.Single();
        var formulaOption = command.Options.Single(option => option.SwitchName == "--formula");
        using (Assert.Multiple())
        {
            await Assert.That(argument.PropertyName).IsEqualTo("FormulaOperand");
            await Assert.That(argument.IsVariadic).IsTrue();
            await Assert.That(formulaOption.PropertyName).IsEqualTo("Formula");
        }
    }

    [Test]
    public async Task Models_Exec_Command_And_Value_Options()
    {
        const string helpText = """
            Usage: brew exec, x [--formulae=formulae] [--sandbox=path] [--deny-network]
            [--] command [args ...]

            Run command in an environment populated by Homebrew formulae.

                  --formulae      Comma-separated formulae to install and add
                                  to PATH before running command.
                  --sandbox       Run command in Homebrew's sandbox, allowing
                                  writes to path and Homebrew's temporary directories.
                  --deny-network  Deny network access from inside the sandbox.
            """;

        var command = await new TestBrewCliScraper().Parse(["brew", "exec"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.PositionalArguments.Select(argument => argument.PropertyName))
                .IsEquivalentTo(["Command", "Arguments"]);
            await Assert.That(command.PositionalArguments[0].IsRequired).IsTrue();
            await Assert.That(command.PositionalArguments[1].IsVariadic).IsTrue();
            await Assert.That(command.Description)
                .IsEqualTo("Run command in an environment populated by Homebrew formulae.");
            await Assert.That(command.Options.Single(option => option.SwitchName == "--formulae").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--sandbox").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--deny-network").IsFlag)
                .IsTrue();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--formulae").Description)
                .IsEqualTo("Comma-separated formulae to install and add to PATH before running command.");
        }
    }

    [Test]
    public async Task Models_Info_Value_Options_From_Wrapped_Descriptions()
    {
        const string helpText = """
            Usage: brew info, abv [options] [formula|cask ...]

            Display brief statistics for your Homebrew installation. If a formula or
            cask is provided, show summary of information about it.

                  --analytics  List global Homebrew analytics data.
                  --days       How many days of analytics data to retrieve.
                               The value for days must be 30, 90 or 365.
                  --category   Which type of analytics data to retrieve. The
                               value for category must be install or build-error.
                  --json       Print a JSON representation. Currently the
                               default value for version is v1.
                  --installed  Output an inventory. If --json=v2 is passed,
                               include installed casks.
            """;

        var command = await new TestBrewCliScraper().Parse(["brew", "info"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Description)
                .IsEqualTo("Display brief statistics for your Homebrew installation. If a formula or cask is provided, show summary of information about it.");
            await Assert.That(command.Options.Single(option => option.SwitchName == "--analytics").IsFlag)
                .IsTrue();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--days").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--category").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--json").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--installed").IsFlag)
                .IsTrue();
        }
    }

    [Test]
    public async Task Preserves_Multiline_Description_Containing_Option_And_Flag_File_Text()
    {
        const string helpText = """
            Usage: brew create [options] URL

            Generate a formula or, with --cask, a cask for the downloadable file at URL
            and open it in the editor.

                  --HEAD      Indicate that URL points to the package's
                              repository rather than a file.
                  --set-name  Explicitly set the name of the new formula
                              or cask.
            """;

        var command = await new TestBrewCliScraper().Parse(["brew", "create"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Description)
                .IsEqualTo("Generate a formula or, with --cask, a cask for the downloadable file at URL and open it in the editor.");
            await Assert.That(command.Options.Single(option => option.SwitchName == "--HEAD").IsFlag)
                .IsTrue();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--set-name").IsFlag)
                .IsFalse();
        }
    }

    [Test]
    public async Task Discovers_Colon_Delimited_Bundle_Subcommands()
    {
        const string helpText = """
            Usage: brew bundle [subcommand]

            Subcommands:
              sh:
                Run your shell in a brew bundle exec environment.
              install:
                Install dependencies from the Brewfile.
              exec:
                Run an external command.

              -h, --help  Show this message.
            """;

        var subcommands = new TestBrewCliScraper().GetSubcommands(helpText);

        await Assert.That(subcommands).IsEquivalentTo(["sh", "install", "exec"]);
    }

    [Test]
    public async Task Models_Bundle_Child_Options_From_Wrapped_Help()
    {
        const string helpText = """
            Usage: brew bundle [install|upgrade]:
                Install and upgrade dependencies from the Brewfile.

                  --file             Read from or write to the Brewfile from
                                     this location.
                  --no-upgrade       Do not run brew upgrade.
                  --upgrade-formulae, --upgrade-formula
                                     Run brew upgrade on these comma-separated formulae.
                  --jobs             Run up to this many formula installations in
                                     parallel. Use auto for the number of CPU cores.
                  --zap              Use zap instead of uninstall.
            """;

        var command = await new TestBrewCliScraper().Parse(["brew", "bundle", "install"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Description)
                .IsEqualTo("Install and upgrade dependencies from the Brewfile.");
            await Assert.That(command.Options.Single(option => option.SwitchName == "--file").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--no-upgrade").IsFlag)
                .IsTrue();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--upgrade-formulae").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--jobs").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--zap").IsFlag)
                .IsTrue();
        }
    }

    [Test]
    public async Task Ignores_Description_For_Unrelated_Usage()
    {
        const string helpText = """
            Usage: brew install-bundler-gems [--groups=]

            Install Homebrew's Bundler gems.
            """;

        var command = await new TestBrewCliScraper().Parse(["brew", "rubocop"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Description).IsNull();
            await Assert.That(command.Options).IsEmpty();
        }
    }

    [Test]
    public async Task Ignores_Prerequisite_Options_Before_Matching_Usage()
    {
        const string helpText = """
            Usage: brew install-bundler-gems [--groups=]

                  --groups        Install Bundler gem groups.

            Usage: rubocop [options] [file1, file2, ...]

                -l, --lint        Run only lint cops.
            """;

        var command = await new TestBrewCliScraper().Parse(["brew", "rubocop"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options).Contains(option => option.SwitchName == "--lint");
            await Assert.That(command.Options).DoesNotContain(option => option.SwitchName == "--groups");
        }
    }

    [Test]
    public async Task Descriptive_List_And_Writable_Words_Remain_Flags()
    {
        const string helpText = """
            Usage: brew list [options]

                  --formula        List only formulae.
                  --writable       List only writable kegs.
                  --head           Install the HEAD version of a formula.
                  --formulae=LIST  Use a comma-separated list of formulae.
            """;

        var command = await new TestBrewCliScraper().Parse(["brew", "list"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Single(option => option.SwitchName == "--formula").IsFlag)
                .IsTrue();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--writable").IsFlag)
                .IsTrue();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--head").IsFlag)
                .IsTrue();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--formulae").IsFlag)
                .IsFalse();
        }
    }

    [Test]
    public async Task Models_Command_Operands_As_A_Required_Collection()
    {
        const string helpText = "Usage: brew command command [...]";

        var command = await new TestBrewCliScraper().Parse(
            ["brew", "command"],
            helpText);
        var operand = command!.PositionalArguments.Single();

        using (Assert.Multiple())
        {
            await Assert.That(operand.PropertyName).IsEqualTo("Cmd");
            await Assert.That(operand.CSharpType).IsEqualTo("IEnumerable<string>");
            await Assert.That(operand.IsRequired).IsTrue();
            await Assert.That(operand.IsVariadic).IsTrue();
        }
    }

    [Test]
    public async Task Models_Sandbox_Command_After_Writable_Path_Option()
    {
        const string helpText = """
            Usage: brew sandbox-exec [options] -- command [args...]

                  --writable-path=PATH   Add a writable path to the sandbox.
            """;

        var command = await new TestBrewCliScraper().Parse(
            ["brew", "sandbox-exec"],
            helpText);

        var operand = command!.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(operand.PropertyName).IsEqualTo("Command");
            await Assert.That(operand.IsRequired).IsTrue();
            await Assert.That(operand.IsVariadic).IsTrue();
            await Assert.That(operand.PrependOptionTerminator).IsTrue();
            await Assert.That(command.Options.Single().IsFlag).IsFalse();
        }
    }

    [Test]
    public async Task Models_Generate_Zap_Cask_Operand_After_Name_Flag()
    {
        const string helpText = """
            Usage: brew generate-zap [--name] cask_or_name

                  --name   Treat the operand as a cask name.
            """;

        var command = await new TestBrewCliScraper().Parse(
            ["brew", "generate-zap"],
            helpText);

        var operand = command!.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(operand.PropertyName).IsEqualTo("CaskOrName");
            await Assert.That(operand.IsRequired).IsTrue();
        }
    }

    [Test]
    public async Task Models_Unlink_Installed_Formulae_As_A_Required_Collection()
    {
        const string helpText = "Usage: brew unlink [--dry-run] installed_formula [...]";

        var command = await new TestBrewCliScraper().Parse(
            ["brew", "unlink"],
            helpText);
        var operand = command!.PositionalArguments.Single();

        using (Assert.Multiple())
        {
            await Assert.That(operand.PropertyName).IsEqualTo("InstalledFormula");
            await Assert.That(operand.CSharpType).IsEqualTo("IEnumerable<string>");
            await Assert.That(operand.IsRequired).IsTrue();
            await Assert.That(operand.IsVariadic).IsTrue();
        }
    }

    private sealed class TestBrewCliScraper : BrewCliScraper
    {
        public TestBrewCliScraper(ICliCommandExecutor? executor = null)
            : base(
                executor ?? new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<BrewCliScraper>.Instance)
        {
        }

        public override Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }

        public IReadOnlyList<string> GetSubcommands(string helpText) =>
            ExtractSubcommands(helpText).ToArray();
    }

    private sealed class CommandInventoryExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = arguments switch
                {
                    "--help" => "Example usage:\n  brew update",
                    "commands --quiet" => "alpha  beta  foo+bar  baz.qux\n",
                    _ => $"Usage: brew {arguments[..^7]} [options]\n\n  --verbose  Show details.",
                },
                StandardError = arguments == "commands --quiet"
                    ? "warning stale diagnostic"
                    : string.Empty,
            });

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
