using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class KustomizeCliScraperTests
{
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

    private sealed class TestKustomizeCliScraper(ICliCommandExecutor? executor = null)
        : KustomizeCliScraper(
            executor ?? new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<KustomizeCliScraper>.Instance)
    {
        protected override int MaxParallelism => 1;

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }

    private sealed class KustomizeHelpExecutor(bool includeBogusChild) : ICliCommandExecutor
    {
        public List<string> Arguments { get; } = [];

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
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
