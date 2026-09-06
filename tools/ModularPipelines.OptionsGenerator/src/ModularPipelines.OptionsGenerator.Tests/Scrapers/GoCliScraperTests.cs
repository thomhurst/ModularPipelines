using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class GoCliScraperTests
{
    [Test]
    public async Task Traverses_Root_And_Nested_Commands_Including_Optionless_Leaves()
    {
        var scraper = CreateScraper(new Dictionary<string, string>
        {
            ["help"] = """
                Usage:
                    go <command> [arguments]

                The commands are:
                    bug         start a bug report
                    mod         module maintenance
                    version     print Go version

                Use "go help <command>" for more information about a command.
                """,
            ["help bug"] = "usage: go bug\n\nBug starts a bug report.",
            ["help build"] = "usage: go build [packages]\n\nBuild compiles packages.",
            ["help version"] = "usage: go version [-json] [file ...]\n\nVersion reports build information.",
            ["help mod"] = """
                Usage:
                    go mod <command> [arguments]

                The commands are:
                    graph       print module requirement graph
                    verify      verify dependencies have expected content

                Use "go help mod <command>" for more information about a command.
                """,
            ["help mod graph"] = "usage: go mod graph [-go=version] [-x]\n\nGraph prints the module graph.",
            ["help mod verify"] = "usage: go mod verify\n\nVerify checks cached dependencies.",
        });

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo([
                "go bug",
                "go mod",
                "go mod graph",
                "go mod verify",
                "go version",
            ]);
    }

    [Test]
    public async Task Expands_Shared_Build_Flags_For_Affected_Commands()
    {
        const string buildHelp = """
            usage: go build [-o output] [build flags] [packages]

            The build flags are shared by the build, clean, get, install, list, run,
            and test commands:

                -race
                    enable data race detection.
                -cover
                    enable code coverage instrumentation.
                -json
                    emit build output as JSON.
                -tags tag,list
                    a comma-separated list of build tags.
                -ldflags '[pattern=]arg list'
                    arguments to pass to the linker.
                -mod mode
                    module download mode.
                -trimpath
                    remove file system paths.
            """;
        var scraper = CreateScraper(new Dictionary<string, string>
        {
            ["help"] = """
                Usage:
                    go <command> [arguments]

                The commands are:
                    build       compile packages and dependencies
                    clean       remove object files
                    fix         apply fixes
                    generate    generate Go files
                    vet         report suspicious constructs

                Use "go help <command>" for more information about a command.
                """,
            ["help build"] = buildHelp,
            ["help clean"] = "usage: go clean [-cache]\n\nClean removes cached files.",
            ["help fix"] = "usage: go fix [build flags] [-fixtool prog] [packages]\n\nFix applies fixes.",
            ["help generate"] = "usage: go generate [build flags] [-run regexp] [file.go ...]\n\nGenerate runs generators.",
            ["help vet"] = "usage: go vet [build flags] [-vettool prog] [packages]\n\nVet reports suspicious constructs.",
        }, new HashSet<string>(StringComparer.Ordinal)
        {
            "clean -race", "clean -tags", "clean -ldflags", "clean -mod", "clean -trimpath",
            "fix -tags", "fix -mod",
            "generate -race", "generate -tags", "generate -ldflags", "generate -mod", "generate -trimpath",
            "vet -tags", "vet -ldflags", "vet -mod",
        });
        var commands = await ScrapeAsync(scraper);
        var clean = commands.Single(command => command.FullCommand == "go clean");

        using (Assert.Multiple())
        {
            foreach (var (commandName, commandOption) in new[]
                     {
                         ("fix", "-fixtool"),
                         ("generate", "-run"),
                         ("vet", "-vettool"),
                     })
            {
                var command = commands.Single(item => item.FullCommand == $"go {commandName}");
                await Assert.That(command.Options.Any(option => option.SwitchName == commandOption))
                    .IsTrue();
            }

            await Assert.That(clean.Options.Any(option => option.SwitchName == "-race"))
                .IsTrue();
            await Assert.That(commands.Single(command => command.FullCommand == "go fix").Options
                    .Any(option => option.SwitchName == "-tags"))
                .IsTrue();
            await Assert.That(commands.Single(command => command.FullCommand == "go fix").Options
                    .Any(option => option.SwitchName == "-race"))
                .IsFalse();
            await Assert.That(commands.Single(command => command.FullCommand == "go generate").Options
                    .Any(option => option.SwitchName == "-race"))
                .IsTrue();
            await Assert.That(commands.Single(command => command.FullCommand == "go vet").Options
                    .Any(option => option.SwitchName == "-trimpath"))
                .IsFalse();
            await Assert.That(commands
                    .Where(command => command.FullCommand is "go fix" or "go generate" or "go vet")
                    .All(command => command.Options.All(option => option.SwitchName != "-cover")))
                .IsTrue();
            await Assert.That(commands.Single(command => command.FullCommand == "go generate").Options
                    .Any(option => option.SwitchName == "-json"))
                .IsFalse();
        }
    }

    [Test]
    public async Task Loads_Direct_Doc_Flags()
    {
        var scraper = CreateScraper(new Dictionary<string, string>
        {
            ["help"] = """
                Usage:
                    go <command> [arguments]

                The commands are:
                    build       compile packages
                    doc         show documentation
                """,
            ["help build"] = """
                usage: go build [build flags]

                The build flags are shared by the build command:

                    -C dir
                        Change to dir before running the command.
                    -race
                        enable data race detection.
                """,
            ["help doc"] = "usage: go doc [doc flags] [doc]\n\nDoc shows documentation.",
            ["doc -h"] = "Usage of go doc:\n  -C dir\n    change to dir before running command\n  -c\tsymbol matching honors case (paths not affected)\n  -u\tshow unexported symbols as well as exported",
        });

        var doc = (await ScrapeAsync(scraper)).Single(command => command.FullCommand == "go doc");
        var workingDirectory = doc.Options.Single(option => option.SwitchName == "-C");
        var caseSensitive = doc.Options.Single(option => option.SwitchName == "-c");
        var unexported = doc.Options.Single(option => option.SwitchName == "-u");

        using (Assert.Multiple())
        {
            await Assert.That(workingDirectory.IsFlag).IsFalse();
            await Assert.That(workingDirectory.Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
            await Assert.That(caseSensitive.IsFlag).IsTrue();
            await Assert.That(caseSensitive.CSharpType).IsEqualTo("bool?");
            await Assert.That(unexported.IsFlag).IsTrue();
            await Assert.That(unexported.CSharpType).IsEqualTo("bool?");
            await Assert.That(doc.Options.Any(option => option.SwitchName == "-race")).IsFalse();
        }
    }

    [Test]
    public async Task Parses_Repeatable_Edit_Operations_In_Documented_Order()
    {
        const string helpText = """
            usage: go mod edit [editing flags] [-fmt|-print|-json] [go.mod]

            The -module flag changes the module path.
            The -C flag changes to the named directory before running the command.

            The -require=path@version and -droprequire=path flags
            add and drop a requirement.

            The -replace=old[@v]=new[@v] and -dropreplace=old[@v] flags
            add and drop a replacement.

            The -require, -droprequire, -replace, and -dropreplace editing flags may be repeated,
            and the changes are applied in the order given.
            """;
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(["go", "mod", "edit"], helpText);

        using (Assert.Multiple())
        {
            var switchNames = command!.Options.Select(option => option.SwitchName).ToHashSet();
            await Assert.That(new[] { "-module", "-require", "-droprequire", "-replace", "-dropreplace" }
                    .All(switchNames.Contains))
                .IsTrue();
            await Assert.That(command.Options
                    .Where(option => option.SwitchName is "-require" or "-droprequire" or "-replace" or "-dropreplace")
                    .All(option => option.AcceptsMultipleValues && option.CSharpType == "IEnumerable<string>?"))
                .IsTrue();

            var module = command.Options.Single(option => option.SwitchName == "-module");
            await Assert.That(module.IsFlag).IsFalse();
            await Assert.That(module.CSharpType).IsEqualTo("string?");
            await Assert.That(module.ValueSeparator).IsEqualTo("=");

            var workingDirectory = command.Options.Single(option => option.SwitchName == "-C");
            await Assert.That(workingDirectory.IsFlag).IsFalse();
            await Assert.That(workingDirectory.CSharpType).IsEqualTo("string?");
            await Assert.That(workingDirectory.ValueSeparator).IsEqualTo(" ");

            var orderedEdits = command.PositionalArguments.Single(argument =>
                argument.PropertyName == "OrderedEdits");
            await Assert.That(orderedEdits.CSharpType)
                .IsEqualTo("IEnumerable<GoEditOperation>?");
            await Assert.That(orderedEdits.Phase).IsEqualTo(CommandLinePhase.Normal);
            await Assert.That(orderedEdits.IsVariadic).IsTrue();
        }
    }

    [Test]
    public async Task Adds_Ordered_Edit_Operations_To_Work_Edit()
    {
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(
                ["go", "work", "edit"],
                "usage: go work edit [-use=path] [-dropuse=path] [go.work]");

        var orderedEdits = command!.PositionalArguments.Single(argument =>
            argument.PropertyName == "OrderedEdits");
        await Assert.That(orderedEdits.CSharpType)
            .IsEqualTo("IEnumerable<GoEditOperation>?");
        await Assert.That(orderedEdits.Phase).IsEqualTo(CommandLinePhase.Normal);
    }

    [Test]
    public async Task Preserves_Case_Sensitive_Go_Flags()
    {
        var scraper = CreateScraper(new Dictionary<string, string>
        {
            ["help"] = """
                Usage:
                    go <command> [arguments]

                The commands are:
                    build       compile packages
                    test        test packages
                """,
            ["help build"] = """
                usage: go build [build flags] [packages]

                The build flags are shared by the build and test commands:

                    -C dir
                        Change to dir before running the command.
                """,
            ["help test"] = """
                usage: go test [-c] [build/test flags] [packages]

                    -c
                        Compile the test binary but do not run it.
                """,
            ["help testflag"] = "The following flags are recognized by the 'go test' command:",
        }, new HashSet<string>(StringComparer.Ordinal) { "test -C" });

        var commands = await ScrapeAsync(scraper);
        var test = commands.Single(command => command.FullCommand == "go test");
        var compileOnly = test.Options.Single(option => option.SwitchName == "-c");
        var workingDirectory = test.Options.Single(option => option.SwitchName == "-C");

        using (Assert.Multiple())
        {
            await Assert.That(compileOnly.IsFlag).IsTrue();
            await Assert.That(compileOnly.CSharpType).IsEqualTo("bool?");
            await Assert.That(workingDirectory.IsFlag).IsFalse();
            await Assert.That(workingDirectory.CSharpType).IsEqualTo("string?");
            await Assert.That(workingDirectory.Description).Contains("Change to dir");
            await Assert.That(workingDirectory.Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
            await Assert.That(compileOnly.PropertyName).IsEqualTo("LowerC");
            await Assert.That(workingDirectory.PropertyName).IsEqualTo("UpperC");
            await Assert.That(test.Options.Select(option => option.PropertyName).Distinct(StringComparer.Ordinal).Count())
                .IsEqualTo(test.Options.Count);
        }
    }

    [Test]
    public async Task Preserves_Hyphen_Led_Description_Continuations()
    {
        const string helpText = """
            usage: go test [-fuzz regexp]

                -fuzz regexp
                    Run the fuzz test matching regexp and minimize it using
                    -fuzzminimizetime 1h30s).
            """;
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(["go", "test"], helpText);

        await Assert.That(command!.Options.Single(option => option.SwitchName == "-fuzz").Description)
            .Contains("-fuzzminimizetime 1h30s).");
    }

    [Test]
    public async Task Treats_Equivalent_Tab_Indentation_As_An_Option_Boundary()
    {
        var helpText = string.Join('\n',
        [
            "usage: go doc [doc flags]",
            string.Empty,
            "\t-ex",
            "\t\tInclude executable examples.",
            "  \t-http",
            "\t\tServe HTML docs over HTTP.",
            "\t-short",
            "\t\tShow one-line representations.",
        ]);
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(["go", "doc"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["-ex", "-http", "-short"]);
            await Assert.That(command.Options.Single(option => option.SwitchName == "-ex").Description)
                .DoesNotContain("-http");
        }
    }

    [Test]
    public async Task Parses_Tool_Shared_Build_Flag_Declaration()
    {
        const string helpText = """
            usage: go tool [-n] command [args...]

            Tool also provides the -C, -overlay, and -modcacherw build flags.
            """;
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(["go", "tool"], helpText);

        using (Assert.Multiple())
        {
            var workingDirectory = command!.Options.Single(option => option.SwitchName == "-C");
            await Assert.That(workingDirectory.IsFlag).IsFalse();
            await Assert.That(workingDirectory.ValueSeparator).IsEqualTo(" ");
            await Assert.That(workingDirectory.Phase).IsEqualTo(CommandLinePhase.EarlyOperand);

            var overlay = command.Options.Single(option => option.SwitchName == "-overlay");
            await Assert.That(overlay.IsFlag).IsFalse();
            await Assert.That(overlay.ValueSeparator).IsEqualTo(" ");

            await Assert.That(command.Options.Single(option => option.SwitchName == "-modcacherw").IsFlag)
                .IsTrue();
        }
    }

    [Test]
    public async Task Parses_Fmt_Mod_Flag_From_Prose()
    {
        const string helpText = """
            usage: go fmt [-n] [-x] [packages]

            The -n flag prints commands that would be executed.
            The -x flag prints commands as they are executed.

            The -mod flag's value sets which module download mode to use.
            """;
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(["go", "fmt"], helpText);

        var option = command!.Options.Single(item => item.SwitchName == "-mod");
        await Assert.That(option.IsFlag).IsFalse();
    }

    [Test]
    public async Task Usage_Value_Declarations_Win_Over_Prose_Flag_References()
    {
        var cases = new[]
        {
            (Path: new[] { "go", "list" }, Help: "usage: go list [-f format]\n\nThe -f flag controls formatting.", Switch: "-f", Separator: " "),
            (Path: new[] { "go", "mod", "tidy" }, Help: "usage: go mod tidy [-compat=version]\n\nThe -compat flag selects compatibility.", Switch: "-compat", Separator: "="),
            (Path: new[] { "go", "mod", "download" }, Help: "usage: go mod download [-reuse=old.json]\n\nThe -reuse flag reuses a prior file.", Switch: "-reuse", Separator: "="),
        };

        foreach (var testCase in cases)
        {
            var command = await CreateScraper(new Dictionary<string, string>())
                .Parse(testCase.Path, testCase.Help);
            var option = command!.Options.Single(item => item.SwitchName == testCase.Switch);

            using (Assert.Multiple())
            {
                await Assert.That(option.IsFlag).IsFalse();
                await Assert.That(option.CSharpType).IsEqualTo("string?");
                await Assert.That(option.ValueSeparator).IsEqualTo(testCase.Separator);
            }
        }
    }

    [Test]
    public async Task Value_Examples_And_Broad_Repeatability_Prose_Drive_Option_Shapes()
    {
        const string helpText = """
            usage: go build [build flags]

            The -buildvcs flag controls version stamping. Use -buildvcs=false to disable it.

            The -overlay file option accepts multiple files.
            """;
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(["go", "build"], helpText);

        using (Assert.Multiple())
        {
            var buildVcs = command!.Options.Single(item => item.SwitchName == "-buildvcs");
            await Assert.That(buildVcs.IsFlag).IsFalse();
            await Assert.That(buildVcs.ValueSeparator).IsEqualTo("=");

            var overlay = command.Options.Single(item => item.SwitchName == "-overlay");
            await Assert.That(overlay.AcceptsMultipleValues).IsTrue();
            await Assert.That(overlay.CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    [Test]
    public async Task Parses_Each_Prose_Option_Sentence_Independently()
    {
        const string helpText = """
            usage: go fmt [packages]

            Fmt formats packages. The -n flag prints commands that would be executed.
            The -x flag prints commands as they are executed.
            """;
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(["go", "fmt"], helpText);

        var noExecute = command!.Options.Single(item => item.SwitchName == "-n");
        var trace = command.Options.Single(item => item.SwitchName == "-x");

        await Assert.That(noExecute.Description).DoesNotContain("-x");
        await Assert.That(trace.Description).Contains("-x flag prints commands");
    }

    [Test]
    public async Task Structured_Descriptions_Win_Over_Later_Prose()
    {
        const string helpText = """
            usage: go build [-asmflags value]

                -asmflags value
                    Pass arguments to the assembler.

            The -asmflags, -gccgoflags, -gcflags, and -ldflags flags accept a space-separated list of arguments.
            """;
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(["go", "build"], helpText);

        await Assert.That(command!.Options.Single(option => option.SwitchName == "-asmflags").Description)
            .IsEqualTo("Pass arguments to the assembler.");
    }

    [Test]
    public async Task Loads_Only_Dynamically_Shared_Build_Flags()
    {
        const string buildHelp = """
            usage: go build [-o output] [build flags] [packages]

                -o output
                    write output to file.

            The build flags are shared by the build and vet commands:

                -race
                    enable data race detection.
            """;
        var scraper = CreateScraper(new Dictionary<string, string>
        {
            ["help"] = """
                Usage:
                    go <command> [arguments]

                The commands are:
                    build       compile packages
                    vet         report suspicious constructs
                """,
            ["help build"] = buildHelp,
            ["help vet"] = "usage: go vet [packages]\n\nVet reports suspicious constructs.",
        }, new HashSet<string>(StringComparer.Ordinal) { "vet -race" });

        var commands = await ScrapeAsync(scraper);
        var vet = commands.Single(command => command.FullCommand == "go vet");

        await Assert.That(vet.Options.Any(option => option.SwitchName == "-race")).IsTrue();
        await Assert.That(vet.Options.Any(option => option.SwitchName == "-o")).IsFalse();
    }

    [Test]
    public async Task Loads_Dedicated_Test_Flag_Help()
    {
        var scraper = CreateScraper(new Dictionary<string, string>
        {
            ["help"] = """
                Usage:
                    go <command> [arguments]

                The commands are:
                    build       compile packages
                    test        test packages
                """,
            ["help build"] = """
                usage: go build [build flags] [packages]

                The build flags are shared by the build and test commands:

                    -race
                        enable data race detection.
                """,
            ["help test"] = """
                usage: go test [build/test flags] [packages] [build/test flags & test binary flags]

                See 'go help testflag' for details.
                """,
            ["help testflag"] = string.Join('\n',
            [
                "Profiling output can be inspected with pprof. The -sample_index=alloc_space",
                "and -show_bytes options of pprof control presentation.",
                string.Empty,
                "The following flags are recognized by the 'go test' command:",
                string.Empty,
                "The test binary accepts the following flags:",
                string.Empty,
                "\t-run regexp",
                "\t    Run only matching tests.",
                "\t-count n",
                "\t    Run tests n times.",
                "\t-timeout d",
                "\t    Panic after duration d.",
                "\t-bench regexp",
                "\t    Run matching benchmarks.",
                "\t-coverprofile cover.out",
                "\t    Write a coverage profile.",
            ]),
        });

        var commands = await ScrapeAsync(scraper);
        var test = commands.Single(command => command.FullCommand == "go test");
        var requiredOptions = new[] { "-run", "-count", "-timeout", "-bench", "-coverprofile" };

        await Assert.That(requiredOptions.All(switchName =>
                test.Options.Any(option => option.SwitchName == switchName && !option.IsFlag)))
            .IsTrue();
        await Assert.That(requiredOptions.All(switchName =>
                !string.IsNullOrWhiteSpace(test.Options.Single(option => option.SwitchName == switchName).Description)))
            .IsTrue();
        await Assert.That(test.Options.Any(option => option.SwitchName is "-sample_index" or "-show_bytes"))
            .IsFalse();
    }

    [Test]
    public async Task Test_Args_Renders_After_Package_Operands()
    {
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(
                ["go", "test"],
                "usage: go test [-args] [packages]\n\nThe -args flag passes the remainder to the test binary.");

        var args = command!.Options.Single(option => option.SwitchName == "-args");
        using (Assert.Multiple())
        {
            await Assert.That(args.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(args.IsFlag).IsFalse();
            await Assert.That(args.GroupValues).IsTrue();
            await Assert.That(args.AcceptsMultipleValues).IsFalse();
            await Assert.That(args.Phase).IsEqualTo(CommandLinePhase.Terminal);
        }
    }

    [Test]
    public async Task Get_U_Accepts_Bare_And_Patch_Values()
    {
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(
                ["go", "get"],
                "usage: go get [-u] [-u=patch] [packages]\n\nThe -u flag updates dependencies. Use -u=patch to update patch releases.");

        var update = command!.Options.Single(option => option.SwitchName == "-u");
        using (Assert.Multiple())
        {
            await Assert.That(update.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
            await Assert.That(update.PropertyType).IsEqualTo("CliOptionValue?");
            await Assert.That(update.IsFlag).IsFalse();
            await Assert.That(update.ValueSeparator).IsEqualTo("=");
        }
    }

    [Test]
    public async Task List_Json_Accepts_Bare_And_Field_Values()
    {
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(
                ["go", "list"],
                "usage: go list [-json] [packages]\n\nThe -json flag can optionally be provided with comma-separated field names.");

        var json = command!.Options.Single(option => option.SwitchName == "-json");
        using (Assert.Multiple())
        {
            await Assert.That(json.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
            await Assert.That(json.PropertyType).IsEqualTo("CliOptionValue?");
            await Assert.That(json.IsFlag).IsFalse();
            await Assert.That(json.ValueSeparator).IsEqualTo("=");
        }
    }

    [Test]
    public async Task Parses_Conditional_Prose_Option_Declarations()
    {
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(
                ["go", "list"],
                "usage: go list [packages]\n\nWhen using -m, the -reuse=old.json flag accepts the result of a previous go list -m -json invocation.");

        var reuse = command!.Options.Single(option => option.SwitchName == "-reuse");
        using (Assert.Multiple())
        {
            await Assert.That(reuse.IsFlag).IsFalse();
            await Assert.That(reuse.ValueSeparator).IsEqualTo("=");
            await Assert.That(reuse.Description).Contains("result of a previous go list");
        }
    }

    [Test]
    public async Task Prose_References_Do_Not_Consume_Operands_As_Option_Values()
    {
        var cases = new[]
        {
            (Path: new[] { "go", "work", "use" }, Help: "usage: go work use [-r] [moddirs]\n\nThe -r flag searches recursively. When -r is used, directories are inspected."),
            (Path: new[] { "go", "version" }, Help: "usage: go version [-json] [file ...]\n\nThe -json flag prints JSON. When -json is used, output is structured."),
            (Path: new[] { "go", "get" }, Help: "usage: go get [-tool] [packages]\n\nThe -tool flag selects tool mode. When -tool is used, packages are installed as tools."),
        };

        foreach (var testCase in cases)
        {
            var command = await CreateScraper(new Dictionary<string, string>())
                .Parse(testCase.Path, testCase.Help);
            var option = command!.Options.Single();

            using (Assert.Multiple())
            {
                await Assert.That(option.IsFlag).IsTrue();
                await Assert.That(option.CSharpType).IsEqualTo("bool?");
                await Assert.That(command.PositionalArguments).Count().IsEqualTo(1);
            }
        }
    }

    [Test]
    public async Task Go_Doc_Operands_Are_Optional_And_Variadic()
    {
        var command = await CreateScraper(new Dictionary<string, string>())
            .Parse(["go", "doc"], "usage: go doc [doc]\n\nDoc shows documentation.");
        var operand = command!.PositionalArguments.Single();

        using (Assert.Multiple())
        {
            await Assert.That(operand.IsRequired).IsFalse();
            await Assert.That(operand.IsVariadic).IsTrue();
            await Assert.That(operand.CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    private static TestGoCliScraper CreateScraper(
        IReadOnlyDictionary<string, string> helpByArguments,
        IReadOnlySet<string>? supportedFlagProbes = null) =>
        new(new StubExecutor(helpByArguments, supportedFlagProbes ?? new HashSet<string>()));

    private static async Task<IReadOnlyList<CliCommandDefinition>> ScrapeAsync(ICliScraper scraper)
    {
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        return commands;
    }

    private sealed class TestGoCliScraper(ICliCommandExecutor executor)
        : GoCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<GoCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }

    private sealed class StubExecutor(
        IReadOnlyDictionary<string, string> helpByArguments,
        IReadOnlySet<string> supportedFlagProbes)
        : ICliCommandExecutor
    {
        private const string ProbeValue = "__modularpipelines_probe__";

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            if (arguments.Contains(ProbeValue, StringComparison.Ordinal))
            {
                var probe = arguments.Replace($"={ProbeValue} -h", string.Empty, StringComparison.Ordinal);
                var supported = supportedFlagProbes.Contains(probe);
                return Task.FromResult(new CliCommandResult
                {
                    ExitCode = 2,
                    StandardOutput = string.Empty,
                    StandardError = supported ? "usage" : "flag provided but not defined",
                });
            }

            if (!helpByArguments.TryGetValue(arguments, out var helpText))
            {
                throw new InvalidOperationException($"Unexpected arguments: {arguments}");
            }

            return Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = helpText,
                StandardError = string.Empty,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
