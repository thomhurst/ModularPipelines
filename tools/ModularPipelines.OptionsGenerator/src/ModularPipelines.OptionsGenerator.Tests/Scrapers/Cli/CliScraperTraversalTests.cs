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
    public async Task SharedTraversal_Fails_When_Declared_Group_Has_No_Children()
    {
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                A broken CLI.

                Usage:
                  fake <command>

                Available Commands:
                  parent:  Manage parent resources
                """,
            ["parent --help"] = """
                Manage parent resources.

                Usage:
                  fake parent <command>

                Available Commands:
                """,
        });
        var scraper = new TestCobraScraper(executor);

        await Assert.That(() => ScrapeForExceptionAssertionAsync(scraper))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("no child commands were extracted");
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

    private static async Task<IReadOnlyList<CliCommandDefinition>?> ScrapeForExceptionAssertionAsync(
        ICliScraper scraper) =>
        await ScrapeAsync(scraper);

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

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
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
