using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class PnpmCliScraperTests
{
    [Test]
    public async Task Wrapped_Descriptions_That_Look_Like_Option_Rows_Stay_Prose()
    {
        // The wrapped "--save-exact  to ..." line deliberately keeps two spaces so it satisfies
        // the option-row pattern; only its column keeps it inside the description.
        const string helpText = """
            Usage: pnpm add <name>

            Options:
              -D, --save-dev                 Save package to your `devDependencies`. Combine with
                                             --save-exact  to pin the installed version.
              -E, --save-exact               Install exact version
            """;

        var command = await new TestPnpmCliScraper().Parse(["pnpm", "add"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--save-dev", "--save-exact"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--save-dev").Description)
                .IsEqualTo("Save package to your `devDependencies`. Combine with --save-exact  to pin the installed version.");
        }
    }

    [Test]
    public async Task Clap_Help_Parses_Options_Described_On_The_Following_Lines()
    {
        // pnpm 12 prints clap's long help: the summary precedes the usage line, each option's
        // description starts on the next line, and [possible values] / [default] trailers follow.
        const string helpText = """
            Generate a Software Bill of Materials (SBOM)

            Usage: pnpm sbom [OPTIONS] --sbom-format <FORMAT>

            Options:
                  --sbom-format <FORMAT>
                      The SBOM output format (required)

                      [possible values: cyclonedx, spdx]

                  --sbom-type <SBOM_TYPE>
                      The component type for the root package (default: library)

                      [default: library]

              -D, --dev
                      Only include "devDependencies"

                  --cpu <CPU>...
                      CPU architectures whose platform-specific optional dependencies should be installed.
                      Repeat or comma-separate for multiple values
            """;

        var command = await new TestPnpmCliScraper().Parse(["pnpm", "sbom"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Description).IsEqualTo("Generate a Software Bill of Materials (SBOM)");
            await Assert.That(command.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--sbom-format", "--sbom-type", "--dev", "--cpu"]);

            var format = command.Options.Single(option => option.SwitchName == "--sbom-format");
            await Assert.That(format.Description).IsEqualTo("The SBOM output format (required)");
            await Assert.That(format.IsFlag).IsFalse();
            await Assert.That(format.CSharpType).IsEqualTo("PnpmSbomSbomFormat?");
            await Assert.That(format.EnumDefinition!.Values.Select(value => value.CliValue))
                .IsEquivalentTo(["cyclonedx", "spdx"]);

            var type = command.Options.Single(option => option.SwitchName == "--sbom-type");
            await Assert.That(type.Description)
                .IsEqualTo("The component type for the root package (default: library)");
            await Assert.That(type.EnumDefinition).IsNull();

            var dev = command.Options.Single(option => option.SwitchName == "--dev");
            await Assert.That(dev.IsFlag).IsTrue();
            await Assert.That(dev.ShortForm).IsEqualTo("-D");

            var cpu = command.Options.Single(option => option.SwitchName == "--cpu");
            await Assert.That(cpu.AcceptsMultipleValues).IsTrue();
            await Assert.That(cpu.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(cpu.Description).IsEqualTo(
                "CPU architectures whose platform-specific optional dependencies should be installed. Repeat or comma-separate for multiple values");

            // The usage operand <FORMAT> belongs to --sbom-format, so no positional argument
            // is generated and the operand-coverage check passes.
            await Assert.That(command.PositionalArguments).IsEmpty();
            await Assert.That(command.Enums.Select(definition => definition.EnumName))
                .IsEquivalentTo(["PnpmSbomSbomFormat"]);
        }
    }

    [Test]
    public async Task Clap_Help_Generates_Audit_Level_With_Its_Possible_Values()
    {
        const string helpText = """
            Checks for known security issues with the installed packages

            Usage: pnpm audit [OPTIONS] [PARAMS]...

            Arguments:
              [PARAMS]...
                      Additional parameters

            Options:
                  --audit-level <AUDIT_LEVEL>
                      Only print advisories with severity greater than or equal to this level

                      [possible values: info, low, moderate, high, critical]

              -P, --prod
                      Only audit "dependencies" and "optionalDependencies"
            """;

        var command = await new TestPnpmCliScraper().Parse(["pnpm", "audit"], helpText);

        using (Assert.Multiple())
        {
            var level = command!.Options.Single(option => option.SwitchName == "--audit-level");
            await Assert.That(level.PropertyName).IsEqualTo("AuditLevel");
            await Assert.That(level.EnumDefinition!.Values.Select(value => value.CliValue))
                .IsEquivalentTo(["info", "low", "moderate", "high", "critical"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--prod").IsFlag).IsTrue();
        }
    }

    [Test]
    public async Task Clap_Possible_Values_List_Documents_Each_Enum_Member()
    {
        // clap switches to a "Possible values:" list when any value carries its own help; a
        // long entry wraps onto the next line and a [default] trailer follows the list.
        const string helpText = """
            Install packages

            Usage: pnpm install [OPTIONS]

            Options:
                  --reporter <REPORTER>
                      Reporter output format

                      Possible values:
                      - default:     Rich visual output: a progress line and a
                                     summary
                      - append-only: One line per update
                      - silent:      No progress output

                      [default: default]

                  --color[=<COLOR>]
                      When to use colors
            """;

        var command = await new TestPnpmCliScraper().Parse(["pnpm", "install"], helpText);

        using (Assert.Multiple())
        {
            var reporter = command!.Options.Single(option => option.SwitchName == "--reporter");
            await Assert.That(reporter.Description).IsEqualTo("Reporter output format");
            await Assert.That(reporter.CSharpType).IsEqualTo("PnpmInstallReporter?");
            await Assert.That(reporter.EnumDefinition!.Values.Select(value => (value.CliValue, value.MemberName, value.Description ?? string.Empty)))
                .IsEquivalentTo(
                [
                    ("default", "Default", "Rich visual output: a progress line and a summary"),
                    ("append-only", "AppendOnly", "One line per update"),
                    ("silent", "Silent", "No progress output"),
                ]);

            // --color[=<COLOR>] takes an optional value attached with '='.
            var color = command.Options.Single(option => option.SwitchName == "--color");
            await Assert.That(color.IsFlag).IsFalse();
            await Assert.That(color.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
            await Assert.That(color.ValueSeparator).IsEqualTo("=");
            await Assert.That(color.Description).IsEqualTo("When to use colors");
        }
    }

    [Test]
    public async Task Clap_Aligned_Layout_Keeps_Possible_Values_Out_Of_The_Description()
    {
        // Narrow commands get clap's aligned layout: the description follows the switches on
        // the same row and the trailer is appended to it, wrapped at the terminal width.
        const string helpText = """
            Usage: pnpm sbom [OPTIONS]

            Options:
                  --sbom-format <FORMAT>  The SBOM output format [possible values: cyclonedx,
                                          spdx]
                  --split                 Generate a separate SBOM for each package
            """;

        var command = await new TestPnpmCliScraper().Parse(["pnpm", "sbom"], helpText);

        using (Assert.Multiple())
        {
            var format = command!.Options.Single(option => option.SwitchName == "--sbom-format");
            await Assert.That(format.Description).IsEqualTo("The SBOM output format");
            await Assert.That(format.EnumDefinition!.Values.Select(value => value.CliValue))
                .IsEquivalentTo(["cyclonedx", "spdx"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--split").IsFlag).IsTrue();
        }
    }

    [Test]
    public async Task Skipped_Help_Row_Consumes_Its_Prose_Before_The_Next_Declaration()
    {
        // clap lists --help under every command. Its block is consumed like any other so a
        // wrapped line that starts with a switch is never read as a declaration.
        const string helpText = """
            Usage: pnpm install [OPTIONS]

            Options:
              -h, --help
                      Print help. Use
                      --loglevel to control output

                  --loglevel <LOGLEVEL>
                      What level of logs to print
            """;

        var command = await new TestPnpmCliScraper().Parse(["pnpm", "install"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName)).IsEquivalentTo(["--loglevel"]);
            await Assert.That(command.Options.Single().Description).IsEqualTo("What level of logs to print");
        }
    }

    [Test]
    public async Task Clap_Root_Help_Lists_Commands()
    {
        const string helpText = """
            Experimental package manager for node.js

            Usage: pnpm [OPTIONS] <COMMAND>

            Commands:
              add             Add a package
              install         Install packages [alias: i]
              recursive       Concurrently runs a command in all subdirectory projects [aliases: multi, m]
              help            Print this message or the help of the given subcommand(s)

            Options:
              -h, --help
                      Print help
            """;

        var subcommands = new TestPnpmCliScraper().Subcommands(helpText);

        await Assert.That(subcommands).IsEquivalentTo(["add", "install", "recursive", "help"]);
    }

    [Test]
    public async Task Parses_Every_Option_In_The_Pnpm_12_Install_Help_Fixture()
    {
        var fixturePath = Path.Combine(
            AppContext.BaseDirectory,
            "Fixtures",
            "Pnpm",
            "pnpm-12.3.4-install-help.txt");
        var helpText = (await File.ReadAllTextAsync(fixturePath)).ReplaceLineEndings("\n");

        var command = await new TestPnpmCliScraper().Parse(["pnpm", "install"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Description).IsEqualTo("Install packages");

            // 71 declarations in the fixture, less the clap-provided --help.
            await Assert.That(command.Options.Count).IsEqualTo(70);
            await Assert.That(command.Options.Where(option => string.IsNullOrWhiteSpace(option.Description)))
                .IsEmpty();
            await Assert.That(command.Options.Where(option => option.AcceptsMultipleValues).Select(option => option.SwitchName))
                .IsEquivalentTo(["--cpu", "--os", "--libc"]);
            await Assert.That(command.Enums.Select(definition => definition.EnumName))
                .IsEquivalentTo(["PnpmInstallNodeLinker", "PnpmInstallReporter", "PnpmInstallLoglevel"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--filter").ShortForm).IsEqualTo("-F");

            // The [alias: --production] trailer stays out of the description.
            await Assert.That(command.Options.Single(option => option.SwitchName == "--prod").Description).IsEqualTo(
                "Install only production dependencies. devDependencies are skipped, and removed if already installed");

            var reporter = command.Options.Single(option => option.SwitchName == "--reporter");
            await Assert.That(reporter.EnumDefinition!.Values.Select(value => value.CliValue))
                .IsEquivalentTo(["default", "append-only", "ndjson", "silent"]);
            await Assert.That(reporter.EnumDefinition.Values.Single(value => value.CliValue == "ndjson").Description)
                .IsEqualTo("Newline-delimited JSON on stderr");

            var color = command.Options.Single(option => option.SwitchName == "--color");
            await Assert.That(color.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
            await Assert.That(color.ValueSeparator).IsEqualTo("=");
        }
    }

    private sealed class TestPnpmCliScraper()
        : PnpmCliScraper(
            new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<PnpmCliScraper>.Instance)
    {
        public async Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            var command = await ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
            if (command is null)
            {
                return null;
            }

            // The shared traversal runs this check after parsing; usage operands that are
            // neither generated nor covered by a value option fail generation.
            command = command with { UsagePositionalArguments = usage.PositionalArguments };
            command.ValidateOperandCoverage();
            return command;
        }

        public IEnumerable<string> Subcommands(string helpText) => ExtractSubcommands(helpText);
    }
}
