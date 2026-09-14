using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class PnpmCliScraperTests
{
    [Test]
    [Arguments("create", false)]
    [Arguments("create", true)]
    [Arguments("dlx", false)]
    [Arguments("dlx", true)]
    public async Task Shortened_Repeatability_Prose_Generates_Cpu_Collections(string subcommand, bool blockLayout)
    {
        var description = "CPU architectures whose platform-tagged optional dependencies the "
            + (subcommand == "dlx" ? "dlx " : string.Empty)
            + "install should keep. Repeat or comma-separate for multiple";
        var declaration = blockLayout
            ? "      --cpu <CPU>\n          " + description + "\n"
            : "      --cpu <CPU>  " + description + "\n";
        var help = $"Usage: pnpm {subcommand} [OPTIONS]\n\nOptions:\n" + declaration;
        var command = (await new TestPnpmCliScraper().Parse(["pnpm", subcommand], help))!;
        var cpu = command.Options.Single();
        await Assert.That(cpu.AcceptsMultipleValues).IsTrue();
        await Assert.That(cpu.PropertyType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(cpu.CollectionSeparator).IsNull();

        var generated = await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "pnpm",
            NamespacePrefix = "Pnpm",
            TargetNamespace = "ModularPipelines.Node",
            OutputDirectory = "src/ModularPipelines.Node",
            Commands = [command],
        });
        var content = generated.Single().Content;
        await Assert.That(content).Contains("[CliOption(\"--cpu\")]");
        await Assert.That(content).Contains("public IEnumerable<string>? Cpu { get; set; }");
    }

    [Test]
    [Arguments(1, false)]
    [Arguments(1, true)]
    [Arguments(21, false)]
    [Arguments(21, true)]
    public async Task Non_Enum_Clap_Values_Preserve_Choices_In_Descriptions(int valueCount, bool blockLayout)
    {
        var choices = string.Join(", ", Enumerable.Range(1, valueCount).Select(value => $"format{value}"));
        var description = $"Select output format. [possible values: {choices}]";
        var option = blockLayout
            ? $"      --format <FORMAT>\n          Select output format.\n          [possible values: {choices}]\n"
            : $"      --format <FORMAT>  {description}\n";
        var help = "Usage: pnpm install [OPTIONS]\n\nOptions:\n" + option;
        var command = (await new TestPnpmCliScraper().Parse(["pnpm", "install"], help))!;

        await Assert.That(command.Options.Single().PropertyType).IsEqualTo("string?");
        await Assert.That(command.Options.Single().EnumDefinition).IsNull();
        await Assert.That(command.Options.Single().Description).IsEqualTo(description);
        await Assert.That(command.Enums).IsEmpty();
    }

    [Test]
    public async Task Non_Enum_Clap_Value_Preserves_Individual_Documentation()
    {
        const string help = """
            Usage: pnpm install [OPTIONS]

            Options:
                  --format <FORMAT>
                      Select output format.
                      Possible values:
                      - json: Machine-readable output.
            """;
        var command = (await new TestPnpmCliScraper().Parse(["pnpm", "install"], help))!;

        await Assert.That(command.Options.Single().EnumDefinition).IsNull();
        await Assert.That(command.Options.Single().Description)
            .IsEqualTo("Select output format. [possible values: json: Machine-readable output.]");
    }

    [Test]
    [Arguments("-x  to change the execution mode.")]
    [Arguments("--example  to change the execution mode.")]
    public async Task Short_Only_Clap_Block_Keeps_Option_Shaped_Wrapped_Prose(string continuation)
    {
        var help = "Usage: pnpm install [OPTIONS]\n\nOptions:\n"
            + "  -r\n          Process recursively. Combine with\n          " + continuation
            + "\n  -C <DIR>\n          Starting directory.\n      --offline\n          Use cached packages.\n";
        var command = (await new TestPnpmCliScraper().Parse(["pnpm", "install"], help))!;

        await Assert.That(command.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["-r", "-C", "--offline"]);
        await Assert.That(command.Options.Single(option => option.SwitchName == "-r").Description)
            .IsEqualTo($"Process recursively. Combine with {continuation}");
        await Assert.That(command.Options.Single(option => option.SwitchName == "-C").Description)
            .IsEqualTo("Starting directory.");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--offline").Description)
            .IsEqualTo("Use cached packages.");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Short_Only_Clap_Options_Preserve_Values_And_Descriptions(bool blockLayout)
    {
        var options = blockLayout
            ? "  -r\n          Process recursively.\n  -C <DIR>\n          Starting directory.\n  -h\n      --offline\n          Use cached packages.\n"
            : "  -r            Process recursively.\n  -C <DIR>      Starting directory.\n  -h            Print help.\n      --offline  Use cached packages.\n";
        var help = "Usage: pnpm install [OPTIONS]\n\nOptions:\n" + options;
        var command = (await new TestPnpmCliScraper().Parse(["pnpm", "install"], help))!;

        await Assert.That(command.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["-r", "-C", "--offline"]);
        var recursive = command.Options.Single(option => option.SwitchName == "-r");
        var directory = command.Options.Single(option => option.SwitchName == "-C");
        await Assert.That(recursive.IsFlag).IsTrue();
        await Assert.That(recursive.Description).IsEqualTo("Process recursively.");
        await Assert.That(directory.IsFlag).IsFalse();
        await Assert.That(directory.PropertyType).IsEqualTo("string?");
        await Assert.That(directory.Description).IsEqualTo("Starting directory.");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--offline").Description)
            .IsEqualTo("Use cached packages.");
    }

    [Test]
    [Arguments("[=<WHEN>]")]
    [Arguments(" [<WHEN>]")]
    public async Task Optional_Clap_Values_Preserve_Choices_Without_Unused_Enums(string valueHint)
    {
        var help = "Usage: pnpm install [OPTIONS]\n\nOptions:\n      --color" + valueHint
            + "\n          Set color behavior.\n          [possible values: always, auto, never]\n";
        var command = (await new TestPnpmCliScraper().Parse(["pnpm", "install"], help))!;
        var option = command.Options.Single();

        await Assert.That(option.PropertyType).IsEqualTo("CliOptionValue?");
        await Assert.That(option.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
        await Assert.That(option.EnumDefinition).IsNull();
        await Assert.That(option.Description).Contains("always");
        await Assert.That(option.Description).Contains("auto");
        await Assert.That(option.Description).Contains("never");
        await Assert.That(command.Enums).IsEmpty();
    }
    [Test]
    [Arguments("[EXPERIMENTAL]", "[EXPERIMENTAL]")]
    [Arguments("[EXPERIMENTAL]\n          Additional details.", "[EXPERIMENTAL] Additional details.")]
    [Arguments("[Preview\n          feature]", "[Preview feature]")]
    [Arguments("[warning: unstable API]", "[warning: unstable API]")]
    [Arguments("[Unclosed note", "[Unclosed note")]
    [Arguments("[default: unfinished", "[default: unfinished")]
    [Arguments("[default: library]", "[default: library]")]
    [Arguments("[alias: format]", "[alias: format]")]
    [Arguments("[aliases: format, output]", "[aliases: format, output]")]
    [Arguments("[env: PNPM_REPORTER=]", "[env: PNPM_REPORTER=]")]
    [Arguments("[default: a long\n          value]", "[default: a long value]")]
    [Arguments("[default: compact]\n          [possible values: compact, verbose]", "[default: compact]")]
    [Arguments("Component type (default: library)\n          [default: library]", "Component type (default: library)")]
    [Arguments("Component type (DEFAULT: library)\n          [default: library]", "Component type (DEFAULT: library)")]
    [Arguments("Component type [default: library]\n          [default: library]", "Component type [default: library]")]
    [Arguments("[default: library]\n          [default: library]", "[default: library]")]
    [Arguments("Component type (default: Library)\n          [default: library]", "Component type (default: Library) [default: library]")]
    [Arguments("Component type (default: library-tools)\n          [default: library]", "Component type (default: library-tools) [default: library]")]
    [Arguments("Component type (default: application)\n          [default: library]", "Component type (default: application) [default: library]")]
    [Arguments("Component type (fallback: library)\n          [default: library]", "Component type (fallback: library) [default: library]")]
    [Arguments("Component type (default: library]\n          [default: library]", "Component type (default: library] [default: library]")]
    [Arguments("Component type (default: a long\n          value)\n          [default: a long\n          value]", "Component type (default: a long value)")]
    [Arguments("Component type (default: foo(bar))\n          [default: foo(bar)]", "Component type (default: foo(bar))")]
    [Arguments("Component type (default: [a, b])\n          [default: [a, b]]", "Component type (default: [a, b])")]
    [Arguments("Component type (default: library)\n          [default: library]\n          [default: library]", "Component type (default: library)")]
    [Arguments("Component type (default: library)\n          [default: library]\n          [possible values: library, application]", "Component type (default: library)")]
    public async Task Clap_Help_Preserves_Bracketed_Prose(string description, string expected)
    {
        foreach (var aligned in new[] { false, true })
        {
            var optionDescription = aligned
                ? "  " + description.Replace("\n          ", " ", StringComparison.Ordinal)
                : "\n          " + description;
            var help = "Usage: pnpm install [OPTIONS]\n\nOptions:\n      --reporter <REPORTER>"
                + optionDescription + "\n      --offline\n          Use cached packages.\n";
            var command = (await new TestPnpmCliScraper().Parse(["pnpm", "install"], help))!;
            await Assert.That(command.Options.Single(option => option.SwitchName == "--reporter").Description)
                .IsEqualTo(expected);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--offline").Description)
                .IsEqualTo("Use cached packages.");
        }
    }

    [Test]
    [Arguments("audit")]
    [Arguments("stage")]
    public async Task Current_Group_Help_Preserves_Variadic_Parameters(string commandName)
    {
        var fixturePath = Path.Combine(AppContext.BaseDirectory, "Fixtures", "Pnpm",
            $"pnpm-12.4.1-{commandName}-help.txt");
        var helpText = await File.ReadAllTextAsync(fixturePath);
        var scraper = new TestPnpmCliScraper();

        var command = await scraper.Parse(["pnpm", commandName], helpText);

        await Assert.That(scraper.Subcommands(helpText)).IsEmpty();
        var argument = command!.PositionalArguments.Single();
        await Assert.That(argument.PropertyName).IsEqualTo("Params");
        await Assert.That(argument.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(argument.IsRequired).IsFalse();
        await Assert.That(argument.IsVariadic).IsTrue();
        await Assert.That(argument.Phase).IsEqualTo(CommandLinePhase.Passthrough);
    }

    [Test]
    [Arguments("--allow-build <ALLOW_BUILD>", "Package names allowed to run lifecycle scripts. May be repeated")]
    [Arguments("--ignore <GHSA>", "Ignore a vulnerability by its GitHub advisory ID. May be repeated")]
    [Arguments("--workspace-packages <WORKSPACE_PACKAGES>", "Glob patterns selecting the workspace's projects. Repeat to add more")]
    public async Task Prose_Can_Declare_Repeatable_Clap_Options(string declaration, string description)
    {
        var helpText = $$"""
            Usage: pnpm audit [OPTIONS]

            Options:
                  {{declaration}}
                      {{description}}
            """;

        var command = await new TestPnpmCliScraper().Parse(["pnpm", "audit"], helpText);

        await Assert.That(command!.Options.Single().AcceptsMultipleValues).IsTrue();
        await Assert.That(command.Options.Single().CSharpType).IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    public async Task Optional_Clap_Value_Can_Be_Space_Separated()
    {
        const string helpText = """
            Usage: pnpm audit [OPTIONS]

            Options:
                  --fix [<METHOD>]
                      Fix the audited vulnerabilities using the specified method.
            """;

        var command = await new TestPnpmCliScraper().Parse(["pnpm", "audit"], helpText);

        await Assert.That(command!.Options.Single().ValueArity).IsEqualTo(CliOptionValueArity.Optional);
        await Assert.That(command.Options.Single().ValueSeparator).IsEqualTo(" ");
    }

    [Test]
    public async Task Boolean_Flags_Do_Not_Generate_Unused_Possible_Value_Enums()
    {
        const string helpText = """
            Usage: pnpm install [OPTIONS]

            Options:
                  --offline
                      Use cached packages.
                      [possible values: true, false]

                  --reporter <REPORTER>
                      Select the output format.
                      [possible values: default, append-only]
            """;

        var command = (await new TestPnpmCliScraper().Parse(["pnpm", "install"], helpText))!;
        var flag = command.Options.Single(option => option.SwitchName == "--offline");
        var reporter = command.Options.Single(option => option.SwitchName == "--reporter");

        await Assert.That(flag.IsFlag).IsTrue();
        await Assert.That(flag.CSharpType).IsEqualTo("bool?");
        await Assert.That(flag.EnumDefinition).IsNull();
        await Assert.That(command.Enums).IsEquivalentTo([reporter.EnumDefinition!]);

        var generatedEnums = await new EnumGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "pnpm",
            NamespacePrefix = "Pnpm",
            TargetNamespace = "ModularPipelines.Node",
            OutputDirectory = "output",
            Commands = [command],
        });
        await Assert.That(generatedEnums).HasSingleItem();
        await Assert.That(generatedEnums.Single().Content).Contains("[EnumValue(\"append-only\")]");
    }

    [Test]
    public async Task Possible_Values_With_Colliding_Member_Names_Are_Preserved()
    {
        const string helpText = """
            Usage: pnpm install [OPTIONS]

            Options:
                  --reporter <REPORTER>
                      Reporter output format

                      [possible values: foo-bar, foo_bar, baz, foo-bar]
            """;

        var command = await new TestPnpmCliScraper().Parse(["pnpm", "install"], helpText);

        await Assert.That(command!.Options.Single().EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["foo-bar", "foo_bar", "baz"]);

        var files = await new EnumGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "pnpm",
            NamespacePrefix = "Pnpm",
            TargetNamespace = "ModularPipelines.Node",
            OutputDirectory = "output",
            Commands = [command],
        });
        var generatedEnum = files.Single().Content;
        await Assert.That(generatedEnum).Contains("[EnumValue(\"foo-bar\")]");
        await Assert.That(generatedEnum).Contains("[EnumValue(\"foo_bar\")]");
        await Assert.That(generatedEnum).Contains("FooBarLowercase");
    }

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

            // Usage lists --sbom-format outside [OPTIONS], so it is required.
            await Assert.That(format.IsRequired).IsTrue();
            await Assert.That(format.CSharpType).IsEqualTo("PnpmSbomSbomFormat?");
            await Assert.That(format.EnumDefinition!.Values.Select(value => value.CliValue))
                .IsEquivalentTo(["cyclonedx", "spdx"]);

            var type = command.Options.Single(option => option.SwitchName == "--sbom-type");
            await Assert.That(type.Description)
                .IsEqualTo("The component type for the root package (default: library)");
            await Assert.That(type.EnumDefinition).IsNull();
            await Assert.That(type.IsRequired).IsFalse();

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
    [Arguments(false)]
    [Arguments(true)]
    public async Task Clap_Possible_Values_List_Documents_Each_Enum_Member(bool proseDocumentsDefault)
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

        var description = proseDocumentsDefault ? "Reporter output format (default: default)" : "Reporter output format";
        var command = await new TestPnpmCliScraper().Parse(
            ["pnpm", "install"], helpText.Replace("Reporter output format", description, StringComparison.Ordinal));

        using (Assert.Multiple())
        {
            var reporter = command!.Options.Single(option => option.SwitchName == "--reporter");
            await Assert.That(reporter.Description).IsEqualTo(proseDocumentsDefault ? description : $"{description} [default: default]");
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
                .IsEquivalentTo(["--cpu", "--os", "--libc", "--filter", "--workspace-packages"]);
            await Assert.That(command.Enums.Select(definition => definition.EnumName))
                .IsEquivalentTo(["PnpmInstallNodeLinker", "PnpmInstallReporter", "PnpmInstallLoglevel"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "--filter").ShortForm).IsEqualTo("-F");

            // Preserve the documented alias even though it is not a separate option.
            await Assert.That(command.Options.Single(option => option.SwitchName == "--prod").Description).IsEqualTo(
                "Install only production dependencies. devDependencies are skipped, and removed if already installed [alias: --production]");

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
