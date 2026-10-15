using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class KustomizeCliScraperTests
{
    // Fixtures are unmodified --help output from the official Kustomize v5.8.2 release.
    // Root inheritance: https://github.com/kubernetes-sigs/kustomize/blob/kustomize/v5.8.2/kustomize/commands/commands.go
    // Persistent stack-trace registration:
    // https://github.com/kubernetes-sigs/kustomize/blob/kustomize/v5.8.2/cmd/config/configcobra/cmds.go
    // Build flags use cmd.Flags(), not PersistentFlags():
    // https://github.com/kubernetes-sigs/kustomize/blob/kustomize/v5.8.2/kustomize/commands/build/build.go
    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Root_Inherits_Only_Stack_Trace(string newline)
    {
        var options = new TestKustomizeCliScraper().ParseGlobals(Fixture("root").ReplaceLineEndings(newline));
        var option = options.Single();
        await Assert.That(option.SwitchName).IsEqualTo("--stack-trace");
        await Assert.That(option.CSharpType).IsEqualTo("bool?");
        await Assert.That(option.IsFlag).IsTrue();
        await Assert.That(option.ShortForm).IsNull();
        await Assert.That(option.IsSecret || option.AcceptsMultipleValues).IsFalse();
    }

    [Test]
    [Arguments("build", "\n")]
    [Arguments("build", "\r\n")]
    [Arguments("edit", "\n")]
    [Arguments("edit", "\r\n")]
    [Arguments("edit-add-configmap", "\n")]
    [Arguments("edit-add-configmap", "\r\n")]
    [Arguments("cfg", "\n")]
    [Arguments("cfg", "\r\n")]
    [Arguments("fn", "\n")]
    [Arguments("fn", "\r\n")]
    public async Task Groups_And_Leaves_Confirm_Global_Shape(string fixture, string newline)
    {
        var scraper = new TestKustomizeCliScraper();
        var root = scraper.ParseGlobals(Fixture("root"));
        var command = (await scraper.Parse(["kustomize", .. fixture.Split('-')], Fixture(fixture).ReplaceLineEndings(newline)))!;
        var inherited = command.Options.Where(option => option.SwitchName == "--stack-trace").ToList();
        await Assert.That(inherited.Count).IsEqualTo(1);
        await Assert.That(CliGlobalOptionMerger.Merge(root, inherited).Count).IsEqualTo(1);
    }

    private static string Fixture(string command) => File.ReadAllText(
        Path.Combine(AppContext.BaseDirectory, "Fixtures", $"kustomize-5.8.2-{command}-help.txt"));

    [Test]
    public async Task Conflicting_Inherited_Shape_Fails_Before_Deduplication()
    {
        var scraper = new TestKustomizeCliScraper(new KustomizeHelpExecutor(false, conflictingGlobal: true));
        // Traversal records parse failures; directly exercise the parser after root discovery.
        await foreach (var _ in scraper.ScrapeAsync())
        {
        }

        await Assert.That(() => scraper.Parse(["kustomize", "build"],
                Fixture("build").Replace("--stack-trace   ", "--stack-trace string   ", StringComparison.Ordinal)))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("--stack-trace");
    }

    [Test]
    public async Task Scraped_Commands_Inherit_Stack_Trace_Once_And_Keep_Local_Options()
    {
        var scraper = new TestKustomizeCliScraper(new KustomizeHelpExecutor(false));
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(scraper.CreateToolDefinition().GlobalOptions.Select(option => option.SwitchName))
            .IsEquivalentTo(["--stack-trace"]);
        await Assert.That(commands.Any(command => command.Options.Any(option => option.SwitchName == "--stack-trace")))
            .IsFalse();

        var build = (await scraper.Parse(["kustomize", "build"], Fixture("build")))!;
        var edit = (await scraper.Parse(["kustomize", "edit", "add", "configmap"], Fixture("edit-add-configmap")))!;
        foreach (var command in new[] { build, edit })
        {
            await Assert.That(command.Options.Any(option => option.SwitchName == "--stack-trace")).IsFalse();
        }

        await Assert.That(build.Options.Any(option => option.SwitchName == "--enable-alpha-plugins" && option.IsFlag)).IsTrue();
        var environment = build.Options.Single(option => option.SwitchName == "--env");
        await Assert.That(environment.ShortForm).IsEqualTo("-e");
        await Assert.That(environment.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(environment.AcceptsMultipleValues).IsTrue();
        await Assert.That(build.Options.Single(option => option.SwitchName == "--output").ShortForm).IsEqualTo("-o");
        await Assert.That(build.PositionalArguments.Single().IsRequired).IsFalse();
        await Assert.That(edit.Options.Single(option => option.SwitchName == "--namespace").CSharpType).IsEqualTo("string?");
        await Assert.That(edit.Options.Single(option => option.SwitchName == "--from-literal").AcceptsMultipleValues).IsTrue();
        await Assert.That(edit.PositionalArguments.Single().IsRequired).IsTrue();
        await Assert.That(build.Options.Any(option => option.SwitchName == "--from-literal")).IsFalse();
        await Assert.That(edit.Options.Any(option => option.SwitchName == "--enable-alpha-plugins")).IsFalse();
    }

    private static readonly string[] SetLeafCommands =
        ["image", "nameprefix", "namespace", "namesuffix", "replicas"];

    [Test]
    public async Task Set_Example_Prose_Does_Not_Recurse()
    {
        var executor = new KustomizeHelpExecutor(includeBogusChild: false);
        var scraper = new TestKustomizeCliScraper(executor);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var leafPaths = SetLeafCommands
            .Select(leaf => $"kustomize edit set {leaf}")
            .ToArray();
        var fullCommands = commands.Select(command => command.FullCommand).ToArray();
        foreach (var leafPath in leafPaths)
        {
            await Assert.That(fullCommands).Contains(leafPath);
        }

        await Assert.That(executor.Arguments.Any(argument =>
                argument.Split(' ').Count(value => value == "set") > 1))
            .IsFalse();
    }

    [Test]
    public async Task Repeated_Command_Path_Fails_Before_Depth_Limit()
    {
        var scraper = new TestKustomizeCliScraper(
            new KustomizeHelpExecutor(includeBogusChild: true));

        async Task Scrape()
        {
            await foreach (var _ in scraper.ScrapeAsync())
            {
            }
        }

        await Assert.That(Scrape)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("Repeated command-path segment 'set'")
            .And.HasMessageContaining("kustomize edit set image set");
    }

    [Test]
    [Arguments("DIR")]
    [Arguments("[path]")]
    public async Task Build_Path_Is_Optional(string operandSyntax)
    {
        var command = await new TestKustomizeCliScraper().Parse(
            ["kustomize", "build"],
            $$"""
              Build a set of KRM resources using a 'kustomization.yaml' file.
              If DIR is omitted, '.' is assumed.

              Usage:
                kustomize build {{operandSyntax}} [flags]

              Flags:
                    --enable-helm   Enable use of the Helm chart inflator generator.
              """);

        var directory = command!.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(directory.IsRequired).IsFalse();
            await Assert.That(directory.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    [Arguments("add", "annotation", true)]
    [Arguments("add", "component", true)]
    [Arguments("add", "configuration", true)]
    [Arguments("add", "generator", true)]
    [Arguments("add", "label", true)]
    [Arguments("add", "resource", true)]
    [Arguments("add", "transformer", true)]
    [Arguments("remove", "component", true)]
    [Arguments("remove", "resource", true)]
    [Arguments("remove", "transformer", true)]
    [Arguments("set", "annotation", true)]
    [Arguments("set", "image", true)]
    [Arguments("set", "label", true)]
    [Arguments("set", "replicas", true)]
    [Arguments("add", "base", false)]
    [Arguments("add", "buildmetadata", false)]
    [Arguments("remove", "annotation", false)]
    [Arguments("remove", "buildmetadata", false)]
    [Arguments("remove", "label", false)]
    [Arguments("set", "buildmetadata", false)]
    [Arguments("set", "nameprefix", false)]
    [Arguments("set", "namespace", false)]
    [Arguments("set", "namesuffix", false)]
    public async Task Edit_Operands_Omitted_From_Usage_Are_Required(
        string verb,
        string noun,
        bool expectedVariadic)
    {
        var command = await new TestKustomizeCliScraper().Parse(
            ["kustomize", "edit", verb, noun],
            $$"""
              Edits the kustomization file.

              Usage:
                kustomize edit {{verb}} {{noun}} [flags]

              Flags:
                -h, --help   help for {{noun}}
              """);

        var operand = command!.PositionalArguments.Single();
        using (Assert.Multiple())
        {
            await Assert.That(operand.IsRequired).IsTrue();
            await Assert.That(operand.IsVariadic).IsEqualTo(expectedVariadic);
        }
    }

    [Test]
    [Arguments("add", "patch")]
    [Arguments("remove", "patch")]
    public async Task Flag_Only_Edit_Commands_Have_No_Operands(string verb, string noun)
    {
        var command = await new TestKustomizeCliScraper().Parse(
            ["kustomize", "edit", verb, noun],
            $$"""
              Usage:
                kustomize edit {{verb}} {{noun}} [flags]

              Flags:
                    --path string   Path to the patch file.
              """);

        await Assert.That(command!.PositionalArguments).IsEmpty();
    }

    [Test]
    [Arguments("string", "", "IEnumerable<string>?", false, false, ",")]
    [Arguments("stringToString", " (default [])", "IReadOnlyList<KeyValue>?", true, false, null)]
    public async Task Create_Map_Option_Rendering_Matches_Cobra_Type(
        string typeHint,
        string defaultDescription,
        string expectedType,
        bool expectedIsKeyValue,
        bool expectedAcceptsMultipleValues,
        string? expectedCollectionSeparator)
    {
        var command = await new TestKustomizeCliScraper().Parse(
            ["kustomize", "create"],
            $$"""
              Usage:
                kustomize create [flags]

              Flags:
                    --annotations {{typeHint}}   Add one or more common annotations.{{defaultDescription}}
                    --labels {{typeHint}}        Add one or more common labels.{{defaultDescription}}
              """);

        var options = command!.Options
            .Where(option => option.SwitchName is "--annotations" or "--labels")
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(options).Count().IsEqualTo(2);
            await Assert.That(options.Select(option => option.CSharpType))
                .IsEquivalentTo([expectedType, expectedType]);
            await Assert.That(options.All(option => option.IsKeyValue == expectedIsKeyValue)).IsTrue();
            await Assert.That(options.All(
                    option => option.AcceptsMultipleValues == expectedAcceptsMultipleValues))
                .IsTrue();
            await Assert.That(options.All(
                    option => option.CollectionSeparator == expectedCollectionSeparator))
                .IsTrue();
        }
    }

    private sealed class TestKustomizeCliScraper(ICliCommandExecutor? executor = null)
        : KustomizeCliScraper(
            executor ?? new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<KustomizeCliScraper>.Instance)
    {
        protected override int MaxParallelism => 1;

        public IReadOnlyList<CliOptionDefinition> ParseGlobals(string helpText) => ParseGlobalOptions(helpText);

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }

    private sealed class KustomizeHelpExecutor(bool includeBogusChild, bool conflictingGlobal = false) : ICliCommandExecutor
    {
        public List<string> Arguments { get; } = [];

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            string? workingDirectory = null,
            CancellationToken cancellationToken = default)
        {
            Arguments.Add(arguments);
            var helpText = arguments switch
            {
                "--help" => CommandGroupHelp("kustomize", "edit"),
                "edit --help" => CommandGroupHelp("kustomize edit", "set"),
                "edit set --help" => CommandGroupHelp(
                    "kustomize edit set",
                    includeBogusChild ? ["image"] : SetLeafCommands),
                "edit set image --help" when includeBogusChild => CommandGroupHelp(
                    "kustomize edit set image",
                    "set"),
                _ when arguments.StartsWith("edit set ", StringComparison.Ordinal)
                    => LeafHelp(arguments.Split(' ')[2]),
                _ => throw new InvalidOperationException($"Unexpected arguments: {arguments}"),
            };

            var flagSection = arguments == "--help" ? "Flags" : "Global Flags";
            var typeHint = arguments != "--help" && conflictingGlobal ? " string" : string.Empty;
            helpText += $"\n\n{flagSection}:\n      --stack-trace{typeHint}   print a stack-trace on error\n";

            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = helpText,
                StandardError = string.Empty,
                ExitCode = 0,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        private static string CommandGroupHelp(string command, params string[] children) => $$"""
            Usage:
              {{command}} [command]

            Available Commands:
            {{string.Join('\n', children.Select(child => $"  {child}    Manage {child}"))}}
            """;

        private static string LeafHelp(string leaf) => $$"""
            Usage:
              kustomize edit set {{leaf}} [flags]

            Examples:

            The command
              set {{leaf}} value
            will add the value,
            and overwrite an existing value.

            Flags:
              -h, --help   help for {{leaf}}
            """;
    }
}
