using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
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

    [Test]
    public async Task Parses_Current_Plain_Clipanion_Sections()
    {
        const string helpText = """
            Run a package in a temporary environment

            Usage

            $ yarn dlx <command> ...

            Options

              -p,--package #0    The package(s) to install before running the command
              -q,--quiet         Only report critical errors instead of printing the full install logs

            Details

            This command installs a package in a temporary environment.
            """;

        var command = await new TestYarnCliScraper().Parse(["yarn", "dlx"], helpText);
        var package = command!.Options.Single(option => option.PropertyName == "Package");

        using (Assert.Multiple())
        {
            await Assert.That(command.Options.Select(option => option.PropertyName))
                .IsEquivalentTo(["Package", "Quiet"]);
            await Assert.That(package.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(package.AcceptsMultipleValues).IsTrue();
            await Assert.That(command.PositionalArguments.Single().Phase)
                .IsEqualTo(CommandLinePhase.Passthrough);
        }
    }

    [Test]
    public async Task Preserves_Clipanion_Option_Arity()
    {
        const string helpText = """
            Usage

            $ yarn workspaces foreach [--immutable] [--include #0] [--since] [-v,--verbose]

            Options

              --immutable       Abort if the lockfile would be modified
              --include #0      An array of glob pattern idents or paths
              --since           Only include workspaces changed since the configured base refs
              -v,--verbose      Increase verbosity up to 2 times
            """;

        var command = await new TestYarnCliScraper().Parse(
            ["yarn", "workspaces", "foreach"],
            helpText);

        var immutable = command!.Options.Single(option => option.PropertyName == "Immutable");
        var include = command.Options.Single(option => option.PropertyName == "Include");
        var since = command.Options.Single(option => option.PropertyName == "Since");
        var verbose = command.Options.Single(option => option.PropertyName == "Verbose");
        using (Assert.Multiple())
        {
            await Assert.That(immutable.IsFlag).IsTrue();
            await Assert.That(include.AcceptsMultipleValues).IsTrue();
            await Assert.That(include.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(since.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
            await Assert.That(since.ValueSeparator).IsEqualTo("=");
            await Assert.That(verbose.IsFlag).IsTrue();
            await Assert.That(verbose.CSharpType).IsEqualTo("int?");
            await Assert.That(verbose.ValidationConstraints!.MaxValue).IsEqualTo(2);
        }
    }

    [Test]
    public async Task Version_Apply_Prerelease_Accepts_An_Optional_Identifier()
    {
        const string helpText = """
            Usage

            $ yarn version apply [--prerelease]

            Options

              --prerelease      Apply the prerelease identifier
            """;

        var command = await new TestYarnCliScraper().Parse(
            ["yarn", "version apply"],
            helpText);
        var prerelease = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(prerelease.IsFlag).IsFalse();
            await Assert.That(prerelease.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
            await Assert.That(prerelease.ValueSeparator).IsEqualTo("=");
        }
    }

    [Test]
    public async Task Init_Install_Accepts_An_Optional_Bundle()
    {
        const string helpText = """
            Usage

            $ yarn init [-i,--install]

            Options

              -i,--install      Initialize with a specific bundle
            """;

        var command = await new TestYarnCliScraper().Parse(["yarn", "init"], helpText);
        var install = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(install.IsFlag).IsFalse();
            await Assert.That(install.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
            await Assert.That(install.ValueSeparator).IsEqualTo("=");
        }
    }

    [Test]
    public async Task Run_Inspect_Options_Accept_Optional_Values()
    {
        const string helpText = """
            Usage

            $ yarn run [--inspect] [--inspect-brk]

            Options

              --inspect         Forward to the Node process
              --inspect-brk     Forward to the Node process and break
            """;

        var command = await new TestYarnCliScraper().Parse(["yarn", "run"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.IsFlag))
                .IsEquivalentTo([false, false]);
            await Assert.That(command.Options.Select(option => option.ValueArity))
                .IsEquivalentTo([CliOptionValueArity.Optional, CliOptionValueArity.Optional]);
            await Assert.That(command.Options.Select(option => option.ValueSeparator))
                .IsEquivalentTo(["=", "="]);
        }
    }

    [Test]
    public async Task Wrapped_Descriptions_That_Look_Like_Option_Rows_Stay_Prose()
    {
        // The wrapped "--exact  to ..." line deliberately keeps two spaces so it satisfies the
        // option-row pattern; only its column keeps it inside the description.
        const string helpText = """
            ━━━ Usage ━━━

            $ yarn add [--json] [-E,--exact] ...

            ━━━ Options ━━━

              --json                     Format the output as an NDJSON stream. Combine with
                                         --exact  to pin the resolved versions.
              -E,--exact                 Don't use any semver modifier on the resolved range
            """;

        var command = await new TestYarnCliScraper().Parse(["yarn", "add"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--json", "--exact"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--json").Description)
                .IsEqualTo("Format the output as an NDJSON stream. Combine with --exact  to pin the resolved versions.");
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
