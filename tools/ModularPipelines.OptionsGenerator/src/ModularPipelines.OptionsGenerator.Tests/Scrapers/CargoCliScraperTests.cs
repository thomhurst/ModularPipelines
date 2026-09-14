using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class CargoCliScraperTests
{
    [Test]
    [Arguments("--quiet")]
    [Arguments("--verbose...")]
    [Arguments("--custom <VALUE>...")]
    public async Task Fully_Typed_Options_Do_Not_Require_A_Manual(string declaration)
    {
        var executor = new CargoHelpExecutor($"Usage: cargo build [OPTIONS]\n\nOptions:\n  {declaration}  Select output.\n", "", 1);
        var scraper = new CargoCliScraper(executor, new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<CargoCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Any(command => command.FullCommand == "cargo build")).IsTrue();
        await Assert.That(executor.ManualRequested).IsFalse();
    }

    [Test]
    [Arguments("-v, --verbose...", "int?")]
    [Arguments("-q, --quiet", "bool?")]
    public async Task Repeated_Clap_Flags_Preserve_Counts(string declaration, string expectedType)
    {
        var help = $"Usage: cargo build [OPTIONS]\n\nOptions:\n  {declaration}  Set output verbosity.\n";
        var command = (await new TestCargoCliScraper().Parse(["cargo", "build"], help))!;
        var option = command.Options.Single();
        await Assert.That(option.IsFlag).IsTrue();
        await Assert.That(option.PropertyType).IsEqualTo(expectedType);
        var generated = await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "cargo",
            NamespacePrefix = "Cargo",
            TargetNamespace = "ModularPipelines.Rust",
            OutputDirectory = "src/ModularPipelines.Rust",
            Commands = [command],
        });
        await Assert.That(generated.Single().Content).Contains($"[CliFlag(\"{option.SwitchName}\", ShortForm = \"{option.ShortForm}\")]");
        await Assert.That(generated.Single().Content).Contains($"public {expectedType} {option.PropertyName} {{ get; set; }}");
    }

    [Test]
    [Arguments("", 0)]
    [Arguments("Manual unavailable", 1)]
    public async Task Missing_Manual_Does_Not_Emit_Incomplete_Command(string manual, int exitCode)
    {
        var executor = new CargoHelpExecutor("Usage: cargo build [OPTIONS]\n\nOptions:\n  --custom <VALUE>  Select values.\n", manual, exitCode);
        var scraper = new CargoCliScraper(executor, new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<CargoCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Any(command => command.FullCommand == "cargo build")).IsFalse();
        await Assert.That(executor.ManualRequested).IsTrue();
    }

    [Test]
    public async Task Manual_Repetition_Uses_Option_Metadata_Without_Switch_Allowlist()
    {
        const string help = "Usage: cargo build [OPTIONS]\n\nOptions:\n  --custom <VALUE>  Select values. [possible values: first, second]\n  --single <VALUE>  Select one value.\n";
        const string manual = "OPTIONS\n       --custom VALUE\n           This option may be specified multiple times.\n\n       --single VALUE\n           Select one value.\n";
        var executor = new CargoHelpExecutor(help, manual);
        var scraper = new CargoCliScraper(executor, new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<CargoCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var build = commands.Single(command => command.FullCommand == "cargo build");
        await Assert.That(GetOption(build, "--custom").PropertyType).IsEqualTo("IEnumerable<CargoBuildCustom>?");
        await Assert.That(GetOption(build, "--single").AcceptsMultipleValues).IsFalse();
    }

    [Test]
    [Arguments("--package")]
    [Arguments("--exclude")]
    [Arguments("--features")]
    [Arguments("--target")]
    [Arguments("--bin")]
    [Arguments("--example")]
    [Arguments("--test")]
    [Arguments("--bench")]
    [Arguments("--config")]
    public async Task Captured_Build_Manual_Preserves_Repeated_Required_Values(string switchName)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "cargo-1.98.1-build-help.txt"));
        var manual = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "cargo-1.98.1-build-manual.txt"));
        var executor = new CargoHelpExecutor(help, manual);
        var scraper = new CargoCliScraper(executor, new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<CargoCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var build = commands.Single(command => command.FullCommand == "cargo build");
        var option = GetOption(build, switchName);
        await Assert.That(option.PropertyType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(option.ValueArity).IsEqualTo(CliOptionValueArity.Required);
        await Assert.That(option.AcceptsMultipleValues).IsTrue();
        await Assert.That(GetOption(build, "--profile").AcceptsMultipleValues).IsFalse();
        var formats = GetOption(build, "--message-format");
        await Assert.That(formats.PropertyType).IsEqualTo("IEnumerable<CargoBuildMessageFormat>?");
        await Assert.That(formats.EnumDefinition!.Values.Select(value => value.CliValue)).Contains("json");
        await Assert.That(formats.EnumDefinition.Values.Select(value => value.CliValue)).Contains("json-diagnostic-short");
        await Assert.That(executor.ManualRequested).IsTrue();
    }

    [Test]
    public async Task Shared_Summary_Preserves_Short_And_Wrapped_Text()
    {
        const string help = "Build\nUse --release for optimized output.\n\nUsage: cargo build [OPTIONS]\n\nOptions:\n  --quiet  Be quiet";
        var command = (await new TestCargoCliScraper().Parse(["cargo", "build"], help))!;
        await Assert.That(command.Description).IsEqualTo("Build Use --release for optimized output.");
    }

    [Test]
    [Arguments("--custom <SPEC>...", true)]
    [Arguments("-F <FEATURES>...", true)]
    [Arguments("--package <SPEC>", false)]
    public async Task Placeholder_Repetition_Determines_Collection_Type(string declaration, bool repeatable)
    {
        var help = $"Usage: cargo build [OPTIONS]\n\nSelection:\n  {declaration}  Select values.\n";
        var command = (await new TestCargoCliScraper().Parse(["cargo", "build"], help))!;
        var option = command.Options.Single();
        await Assert.That(option.AcceptsMultipleValues).IsEqualTo(repeatable);
        await Assert.That(option.PropertyType).IsEqualTo(repeatable ? "IEnumerable<string>?" : "string?");
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(true, true)]
    public async Task Shared_Clap_Trailers_Preserve_Defaults_And_Colliding_Enum_Values(bool inline, bool repeatable)
    {
        var description = inline
            ? "  Select a format. [default: json] [possible values: json, JSON,\n                      foo-bar, foo_bar]"
            : "\n          Select a format.\n          [default: json]\n          [possible values: json, JSON,\n              foo-bar, foo_bar]";
        var help = "Usage: cargo build [OPTIONS]\n\nOutput Selection:\n      --format <FORMAT>"
            + (repeatable ? "..." : "") + description
            + "\n  -h, --help  Print help\n";
        var command = (await new TestCargoCliScraper().Parse(["cargo", "build"], help))!;
        var option = command.Options.Single();
        await Assert.That(option.Description).IsEqualTo("Select a format. [default: json]");
        await Assert.That(option.EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["json", "JSON", "foo-bar", "foo_bar"]);
        await Assert.That(option.EnumDefinition.Values.Select(value => value.MemberName).Distinct().Count()).IsEqualTo(4);
        await Assert.That(option.PropertyType).IsEqualTo(repeatable ? "IEnumerable<CargoBuildFormat>?" : "CargoBuildFormat?");
        await Assert.That(command.Enums).Count().IsEqualTo(1);
    }

    [Test]
    public async Task Large_Choice_Lists_Retain_All_Values_In_Prose()
    {
        var values = string.Join(", ", Enumerable.Range(0, 21).Select(index => $"value{index}"));
        var help = $"Usage: cargo build [OPTIONS]\n\nOutput:\n      --format <FORMAT>  Select format. [possible values: {values}]\n";
        var command = (await new TestCargoCliScraper().Parse(["cargo", "build"], help))!;
        var option = command.Options.Single();
        await Assert.That(option.EnumDefinition).IsNull();
        await Assert.That(option.PropertyType).IsEqualTo("string?");
        await Assert.That(option.Description).IsEqualTo($"Select format. [possible values: {values}]");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Documented_Choices_Preserve_Descriptions_With_Enum_Or_Prose_Fallback(bool singleton)
    {
        var help = "Usage: cargo build [OPTIONS]\n\nOutput:\n      --format <FORMAT>\n          Select format.\n"
            + "          Possible values:\n          - json: Machine-readable\n              output.\n"
            + (singleton ? "" : "          - text: Human-readable output.\n");
        var command = (await new TestCargoCliScraper().Parse(["cargo", "build"], help))!;
        var option = command.Options.Single();
        if (singleton)
        {
            await Assert.That(option.EnumDefinition).IsNull();
            await Assert.That(option.Description).Contains("json: Machine-readable output.");
        }
        else
        {
            await Assert.That(option.EnumDefinition!.Values.Single(value => value.CliValue == "json").Description)
                .IsEqualTo("Machine-readable output.");
            await Assert.That(option.EnumDefinition.Values.Single(value => value.CliValue == "text").Description)
                .IsEqualTo("Human-readable output.");
        }
    }

    [Test]
    [Arguments("[<SPEC>]", " ", false)]
    [Arguments("[=<SPEC>]", "=", true)]
    public async Task Cargo_Values_Preserve_Arity_And_Choice_Documentation(string hint, string separator, bool optional)
    {
        var help = $"Usage: cargo build [OPTIONS]\n\nSelection:\n      --package {hint}\n"
            + "          Select package.\n          [possible values: first, second]\n";
        var command = (await new TestCargoCliScraper().Parse(["cargo", "build"], help))!;
        var option = command.Options.Single();
        await Assert.That(option.PropertyType).IsEqualTo(optional ? "CliOptionValue?" : "CargoBuildPackage?");
        await Assert.That(option.ValueArity).IsEqualTo(optional ? CliOptionValueArity.Optional : CliOptionValueArity.Required);
        await Assert.That(option.ValueSeparator).IsEqualTo(separator);
        if (optional)
        {
            await Assert.That(option.EnumDefinition).IsNull();
            await Assert.That(option.Description).Contains("first, second");
        }
        else
        {
            await Assert.That(option.EnumDefinition!.Values.Select(value => value.CliValue)).IsEquivalentTo(["first", "second"]);
        }
    }

    [Test]
    [Arguments("Aliases:")]
    [Arguments("Note:")]
    [Arguments("Compatibility:")]
    [Arguments("Examples Options:")]
    public async Task Prose_Sections_Do_Not_Create_Options(string heading)
    {
        var helpText = $"Usage: cargo run [OPTIONS]\n\nOptions:\n  --quiet  Suppress output\n\n{heading}\n  --example  This is prose, not an option.";
        var command = await new TestCargoCliScraper().Parse(["cargo", "run"], helpText);

        await Assert.That(command!.Options.Select(option => option.SwitchName)).IsEquivalentTo(["--quiet"]);
    }

    [Test]
    public async Task Command_Options_Heading_Preserves_Real_Options()
    {
        const string helpText = "Usage: cargo run [OPTIONS]\n\nCommand Options:\n  --quiet  Suppress output";
        var command = await new TestCargoCliScraper().Parse(["cargo", "run"], helpText);

        await Assert.That(command!.Options.Single().SwitchName).IsEqualTo("--quiet");
    }

    [Test]
    [Arguments("--package <SPEC>")]
    [Arguments("--package [<SPEC>]")]
    [Arguments("--package=<SPEC>")]
    public async Task Unparsed_Value_Options_Do_Not_Become_Positionals(string usage)
    {
        var helpText = $"""
            Execute a package command

            Usage: cargo run [OPTIONS] {usage}

            Options:
              -p, --package [<SPEC>]  Select a package
              -h, --help            Print help
            """;

        var command = await new TestCargoCliScraper().Parse(["cargo", "run"], helpText);

        await Assert.That(command!.PositionalArguments).IsEmpty();
    }

    [Test]
    public async Task Same_Named_Option_Value_Does_Not_Make_An_Alternative_Positional_Required()
    {
        const string helpText = """
            Execute a package command

            Usage: cargo run [OPTIONS] <PATH>
                   cargo run [OPTIONS] --file <PATH>

            Options:
                  --file <PATH>   Read a file
            """;

        var command = await new TestCargoCliScraper().Parse(["cargo", "run"], helpText);
        var positional = command!.PositionalArguments.Single();

        await Assert.That(positional.IsRequired).IsFalse();
        await Assert.That(positional.CSharpType).IsEqualTo("string?");
    }

    [Test]
    public async Task Option_Values_And_Positionals_With_The_Same_Name_Keep_Distinct_Requiredness()
    {
        const string helpText = """
            Execute a package command

            Usage: cargo run [OPTIONS] --file <PATH> <PATH>
                   cargo run [OPTIONS] <OBJECT>

            Options:
                  --file <PATH>   Read a file
            """;

        var command = await new TestCargoCliScraper().Parse(["cargo", "run"], helpText);
        var positional = command!.PositionalArguments.Single();

        using (Assert.Multiple())
        {
            await Assert.That(positional.PropertyName).IsEqualTo("Path");
            await Assert.That(positional.AssociatedOptionSwitch).IsNull();
            await Assert.That(positional.IsRequired).IsTrue();
            await Assert.That(positional.CSharpType).IsEqualTo("string");
        }
    }

    [Test]
    public async Task Presence_Only_Flags_Do_Not_Require_An_Operand_Missing_From_Another_Form()
    {
        const string helpText = """
            Execute a package command

            Usage: cargo run [OPTIONS] <A> [--verbose] <B>
                   cargo run [OPTIONS] <A>

            Options:
              -v, --verbose       Print detailed output
            """;

        var command = await new TestCargoCliScraper().Parse(["cargo", "run"], helpText);
        var first = command!.PositionalArguments.Single(argument => argument.PropertyName == "A");
        var second = command.PositionalArguments.Single(argument => argument.PropertyName == "B");

        using (Assert.Multiple())
        {
            await Assert.That(first.IsRequired).IsTrue();
            await Assert.That(second.IsRequired).IsFalse();
            await Assert.That(second.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    [Arguments("<A> [--verbose] <B>", "<X> <Y>")]
    [Arguments("<A> --verbose <B>", "<X> <Y>")]
    [Arguments("<A> [--missing] <B>", "<X> <Y>")]
    [Arguments("<A> [-v] <B>", "<X> <Y>")]
    [Arguments("<A> <B>", "<X> [--verbose] <Y>")]
    [Arguments("[--verbose] <A> --file <FILE> <B>", "<X> <Y>")]
    public async Task Presence_Only_Flags_Do_Not_Relax_Renamed_Required_Operands(
        string firstForm,
        string secondForm)
    {
        var helpText = $"""
            Execute a package command

            Usage: cargo run [OPTIONS] {firstForm}
                   cargo run [OPTIONS] {secondForm}

            Options:
              -v, --verbose       Print detailed output
                  --file <FILE>   Read a file
            """;

        var command = await new TestCargoCliScraper().Parse(["cargo", "run"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.PositionalArguments.Select(argument => argument.PropertyName))
                .IsEquivalentTo(["A", "B"]);
            await Assert.That(command.PositionalArguments.All(argument => argument.IsRequired)).IsTrue();
            await Assert.That(command.PositionalArguments.All(argument => argument.CSharpType == "string")).IsTrue();
        }
    }

    [Test]
    public async Task Wrapped_Descriptions_That_Look_Like_Option_Rows_Stay_Prose()
    {
        const string helpText = """
            Compile a local package and all of its dependencies

            Usage: cargo build [OPTIONS]

            Options:
                  --ignore-rust-version
                                      Ignore `rust-version` specification in packages
                  --keep-going        Do not abort the build as soon as there is an error. Implies
                                      --jobs
                  --timings           Output information how long each compilation takes
              -h, --help              Print help
            """;

        var command = await new TestCargoCliScraper().Parse(["cargo", "build"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--ignore-rust-version", "--keep-going", "--timings"]);
            await Assert.That(GetOption(command, "--ignore-rust-version").Description)
                .IsEqualTo("Ignore `rust-version` specification in packages");
            await Assert.That(GetOption(command, "--keep-going").Description)
                .IsEqualTo("Do not abort the build as soon as there is an error. Implies --jobs");
            await Assert.That(GetOption(command, "--timings").Description)
                .IsEqualTo("Output information how long each compilation takes");
        }
    }

    [Test]
    public async Task Options_Under_Custom_Clap_Headings_Are_Parsed()
    {
        // cargo add groups its dependency-source and section switches under "Source:" and
        // "Section:" rather than an "Options" heading; only Arguments/Commands are skipped.
        const string helpText = """
            Add dependencies to a Cargo.toml manifest file

            Usage: cargo add [OPTIONS] <DEP>[@<VERSION>] ...
                   cargo add [OPTIONS] --path <PATH> ...
                   cargo add [OPTIONS] --git <URL> ...

            Arguments:
              [DEP_ID]...
                      Reference to a package to add as a dependency

            Options:
                  --no-default-features
                      Disable the default features
              -h, --help
                      Print help (see a summary with '-h')

            Manifest Options:
                  --manifest-path <PATH>
                      Path to Cargo.toml

            Package Selection:
              -p, --package [<SPEC>]
                      Package to modify

            Source:
                  --path <PATH>
                      Filesystem path to local crate to add
                  --git <URI>
                      Git repository location
                  --branch <BRANCH>
                      Git branch to download the crate from

            Section:
                  --dev
                      Add as development dependency
                  --target <TARGET>
                      Add as dependency to the given target platform
            """;

        var command = await new TestCargoCliScraper().Parse(["cargo", "add"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo([
                    "--no-default-features", "--package", "--manifest-path",
                    "--path", "--git", "--branch", "--dev", "--target",
                ]);
            await Assert.That(GetOption(command, "--path").Description)
                .IsEqualTo("Filesystem path to local crate to add");
            await Assert.That(GetOption(command, "--dev").IsFlag).IsTrue();
        }
    }

    private static CliOptionDefinition GetOption(CliCommandDefinition command, string switchName) =>
        command.Options.Single(option => option.SwitchName == switchName);

    private sealed class CargoHelpExecutor(string help, string manual, int manualExitCode = 0) : ICliCommandExecutor
    {
        public bool ManualRequested { get; private set; }

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            ManualRequested |= arguments == "help build";
            var output = arguments switch
            {
                "--help" => "Usage: cargo [COMMAND]\n\nCommands:\n  build  Compile packages\n",
                "build --help" => help,
                "help build" => manual,
                "--version" => "cargo 1.98.1 (797e8a9bc 2026-08-05)",
                _ => throw new InvalidOperationException($"Unexpected arguments: {arguments}"),
            };
            return Task.FromResult(new CliCommandResult { StandardOutput = output, StandardError = "", ExitCode = arguments == "help build" ? manualExitCode : 0 });
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }

    private sealed class TestCargoCliScraper()
        : CargoCliScraper(
            new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<CargoCliScraper>.Instance)
    {
        protected override Task<string> GetManualHelpTextAsync(string[] commandPath, CancellationToken cancellationToken) =>
            Task.FromResult(string.Empty);

        public async Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var command = await ParseCommandAsync(
                commandPath,
                helpText,
                UsageSynopsisParser.Parse(helpText, commandPath),
                CancellationToken.None);
            return command is null ? null : ApplyIgnoredOptionPolicy(command);
        }
    }
}
