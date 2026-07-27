using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class CliScraperTraversalTests
{
    [Test]
    public async Task SharedTraversal_Discovers_ExecutableParent_And_Children()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                A tool-neutral CLI.

                Usage:
                  fake <command> [flags]

                Available Commands:
                  parent:  Execute or manage parent resources
                """,
            ["parent --help"] = """
                Execute or manage parent resources.

                Usage:
                  fake parent <command> [flags]

                Available Commands:
                  child:  Execute a child command

                Flags:
                  --scope string   Select a scope
                """,
            ["parent child --help"] = """
                Execute a child command.

                Usage:
                  fake parent child [flags]

                Flags:
                  --value string   Supply a value
                """,
        });
        var scraper = new TestCobraScraper(executor);

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(["fake parent", "fake parent child"]);
        await Assert.That(commands.Single(command => command.FullCommand == "fake parent").Options)
            .Contains(option => option.SwitchName == "--scope");
        await Assert.That(executor.Arguments)
            .IsEquivalentTo(["--help", "parent --help", "parent child --help"]);
    }

    [Test]
    public async Task SharedTraversal_Skips_Invalid_Group_And_Continues_With_Sibling()
    {
        var emptyGroupHelp = """
            Manage parent resources.

            Usage:
              fake parent <command>

            Available Commands:
            """.ReplaceLineEndings("\r\n");
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                A broken CLI.

                Usage:
                  fake <command>

                Available Commands:
                  parent:  Manage parent resources
                  sibling: Execute a sibling command
                """,
            ["parent --help"] = emptyGroupHelp,
            ["sibling --help"] = """
                Execute a sibling command.

                Usage:
                  fake sibling [flags]

                Flags:
                  --value string   Supply a value
                """,
        });
        var scraper = new TestCobraScraper(executor);

        await Assert.That(scraper.DeclaresCommandGroup(emptyGroupHelp)).IsTrue();
        await Assert.That(scraper.GetSubcommands(emptyGroupHelp)).IsEmpty();

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(["fake sibling"]);
        await Assert.That(executor.Arguments)
            .IsEquivalentTo(["--help", "parent --help", "sibling --help"]);
    }

    [Test]
    public async Task SharedShapeInference_Models_Documented_Repeatability()
    {
        const string helpText = """
            Execute a command.

            Usage:
              fake execute [flags]

            Flags:
              --tag string   May be specified multiple times
            """;
        var scraper = new TestCobraScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));

        var command = await scraper.Parse(["fake", "execute"], helpText);
        var tag = command!.Options.Single();

        await Assert.That(tag.AcceptsMultipleValues).IsTrue();
        await Assert.That(tag.CSharpType).IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    public async Task SharedTraversal_Skips_One_Invalid_Option_Shape()
    {
        const string helpText = """
            Execute a command.

            Usage:
              fake [flags]

            Flags:
              --tag string   May be specified multiple times
            """;
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = helpText,
        });
        var scraper = new ShapeMismatchScraper(executor);

        var commands = await ScrapeAsync(scraper);

        await Assert.That(commands).IsEmpty();
    }

    [Test]
    public async Task SharedTraversal_Propagates_Invalid_Operand_Coverage()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Execute a command.

                Usage:
                  fake <TARGET>
                """,
        });
        var scraper = new OperandCoverageMismatchScraper(executor);

        async Task Scrape() => await ScrapeAsync(scraper);

        await Assert.That(Scrape)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("no CliPositionalArgument");
    }

    [Test]
    public async Task Shared_Skip_Filter_Preserves_Uppercase_Subcommands()
    {
        var scraper = new ShapeMismatchScraper(new StubExecutor(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));

        await Assert.That(scraper.Skips("SSH")).IsFalse();
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

    private sealed class TestCobraScraper : CobraCliScraper
    {
        public TestCobraScraper(ICliCommandExecutor executor)
            : base(
                executor,
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<TestCobraScraper>.Instance)
        {
        }

        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        protected override int MaxParallelism => 2;

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }

        public bool DeclaresCommandGroup(string helpText) => HelpDeclaresCommandGroup(helpText);

        public IReadOnlyList<string> GetSubcommands(string helpText) => ExtractSubcommands(helpText).ToList();
    }

    private sealed class ShapeMismatchScraper : CliScraperBase
    {
        public ShapeMismatchScraper(ICliCommandExecutor executor)
            : base(
                executor,
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<ShapeMismatchScraper>.Instance)
        {
        }

        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        protected override IEnumerable<string> ExtractSubcommands(string helpText) => [];

        public bool Skips(string subcommand) => IsSkippableSubcommand(subcommand);

        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath,
            string helpText,
            CancellationToken cancellationToken) =>
            Task.FromResult<CliCommandDefinition?>(new CliCommandDefinition
            {
                FullCommand = "fake",
                CommandParts = [],
                ClassName = "FakeOptions",
                ParentClassName = "FakeOptions",
                ToolNamespacePrefix = "Fake",
                Options =
                [
                    new CliOptionDefinition
                    {
                        SwitchName = "--tag",
                        PropertyName = "Tag",
                        CSharpType = "string?",
                        Description = "May be specified multiple times",
                    },
                ],
            });
    }

    private sealed class OperandCoverageMismatchScraper : CliScraperBase
    {
        public OperandCoverageMismatchScraper(ICliCommandExecutor executor)
            : base(
                executor,
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<OperandCoverageMismatchScraper>.Instance)
        {
        }

        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        protected override IEnumerable<string> ExtractSubcommands(string helpText) => [];

        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath,
            string helpText,
            CancellationToken cancellationToken) =>
            Task.FromResult<CliCommandDefinition?>(new CliCommandDefinition
            {
                FullCommand = "fake",
                CommandParts = [],
                ClassName = "FakeOptions",
                ParentClassName = "FakeOptions",
                ToolNamespacePrefix = "Fake",
                Options = [],
            });
    }

    private sealed class StubExecutor(IReadOnlyDictionary<string, string> helpByArguments)
        : ICliCommandExecutor
    {
        private readonly object _gate = new();
        private readonly List<string> _arguments = [];

        public IReadOnlyList<string> Arguments
        {
            get
            {
                lock (_gate)
                {
                    return _arguments.ToArray();
                }
            }
        }

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            lock (_gate)
            {
                _arguments.Add(arguments);
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
