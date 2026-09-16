using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GcloudDispatchTests
{
    // Captured from SDK 585.0.0 in generation run 35058181945's coverage
    // diagnostics. The two leaf dispatch pages were captured from the same SDK locally.
    private static readonly Dictionary<string, string> CapturedHelp = JsonSerializer.Deserialize<Dictionary<string, string>>(
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", "585.0.0", "dispatch-help.json")))!;

    public static IEnumerable<string> DispatchCommandPaths() => CapturedHelp.Keys.Where(path => path != "gcloud");

    [Test]
    [MethodDataSource(nameof(DispatchCommandPaths))]
    public async Task Captured_Dispatch_Commands_Retain_Coverage(string path)
    {
        var commands = await Scrape(new Dictionary<string, string> { [path] = CapturedHelp[path] });
        var command = commands.SingleOrDefault(command => command.FullCommand == path);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.HasOperandTakingUsage).IsFalse();
        await Assert.That(command.PositionalArguments).IsEmpty();
        await Assert.That(command.RequiredAlternativeGroups).IsEmpty();
    }

    [Test]
    public async Task Captured_Dispatch_Tree_Preserves_Nested_Groups_And_Empty_Child_Lists()
    {
        var commands = await Scrape(CapturedHelp);
        var missing = DispatchCommandPaths().Except(commands.Select(command => command.FullCommand)).ToArray();

        await Assert.That(missing).IsEmpty();
        await Assert.That(commands.Any(command => command.FullCommand == "gcloud network-security ull-mirroring-collectors rules")).IsTrue();
    }

    [Test]
    [Arguments("GROUP", "Group")]
    [Arguments("COMMAND", "Command")]
    public async Task Documented_Positional_Arguments_Are_Not_Dispatch_Selectors(string operand, string property)
    {
        var commands = await GcloudResourceArgumentTests.ScrapeFixture("example describe", $"""
            SYNOPSIS
                gcloud example describe {operand} [GCLOUD_WIDE_FLAG ...]
            POSITIONAL ARGUMENTS
                 {operand}
                    The resource to describe.
            """);

        var argument = commands.Single().PositionalArguments.Single();
        await Assert.That(argument.PropertyName).IsEqualTo(property);
        await Assert.That(argument.IsRequired).IsTrue();
    }

    [Test]
    public async Task Default_Placeholder_Policy_Preserves_A_Group_Operand()
    {
        var usage = UsageSynopsisParser.Parse("Usage: tool describe GROUP", ["tool", "describe"]);
        var normalized = UsageSynopsisParser.RemoveCommandGroupPlaceholders(usage);

        await Assert.That(normalized.PositionalArguments.Single().PropertyName).IsEqualTo("Group");
        await Assert.That(normalized.HasOperandTokens).IsTrue();
    }

    private static async Task<List<CliCommandDefinition>> Scrape(IReadOnlyDictionary<string, string> help)
    {
        var scraper = new GcloudCliScraper(new DispatchExecutor(help),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<GcloudCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }
        return commands;
    }

    private sealed class DispatchExecutor(IReadOnlyDictionary<string, string> help) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            var path = "gcloud " + arguments.Replace("--help", "", StringComparison.Ordinal).Trim();
            path = path.TrimEnd();
            if (!help.TryGetValue(path, out var output))
            {
                // Supply only missing ancestors needed to reach the captured page.
                // Uncaptured descendants have no help and are outside this fixture.
                var prefix = path + " ";
                var children = help.Keys.Where(key => key.StartsWith(prefix, StringComparison.Ordinal))
                    .Select(key => key[prefix.Length..].Split(' ')[0]).Distinct().ToArray();
                output = children.Length == 0 ? "" : "COMMANDS\n" + string.Join('\n', children.Select(child => "     " + child));
            }
            return Task.FromResult(new CliCommandResult { ExitCode = 0, StandardOutput = output, StandardError = "" });
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
