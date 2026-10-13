using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class GitCliScraperTests
{
    [Test]
    [Timeout(30_000)]
    public async Task Queued_Root_Lookup_Reuses_Cached_Help_Without_Changing_Provenance(CancellationToken cancellationToken)
    {
        var executor = new BlockingRootHelpExecutor();
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        using var scraper = new RootHelpProbe(executor, cache);
        var first = scraper.Fetch(cancellationToken);
        try
        {
            await executor.DiscoveryStarted.Task.WaitAsync(cancellationToken);
            // Fetch runs through the empty-cache check and queues before discovery is released.
            var second = scraper.Fetch(cancellationToken);
            executor.ReleaseDiscovery.TrySetResult();

            var help = await Task.WhenAll(first, second);
            await Assert.That(help[0]).Contains("usage: git");
            await Assert.That(help[1]).IsEqualTo(help[0]);
            await Assert.That(executor.CommandListCalls).IsEqualTo(1);
            await Assert.That(executor.RootUsageCalls).IsEqualTo(1);
            await Assert.That(scraper.UnavailableHelpPaths).IsEmpty();
        }
        finally
        {
            executor.ReleaseDiscovery.TrySetResult();
            await first;
        }
    }

    private sealed class BlockingRootHelpExecutor : ICliCommandExecutor
    {
        public TaskCompletionSource DiscoveryStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource ReleaseDiscovery { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public int CommandListCalls { get; private set; }
        public int RootUsageCalls { get; private set; }

        public async Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            if (arguments == "help -a")
            {
                if (++CommandListCalls > 1)
                {
                    return Result("discovery failed", exitCode: 128);
                }

                DiscoveryStarted.TrySetResult();
                await ReleaseDiscovery.Task.WaitAsync(cancellationToken);
                return Result("Main Porcelain Commands\n   branch                  List branches");
            }

            if (arguments == "-h")
            {
                RootUsageCalls++;
                return Result("usage: git [-C <path>] <command>", exitCode: 129);
            }

            throw new InvalidOperationException($"Unexpected help command: {arguments}");
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }

    [Test]
    [Arguments(1, "wrapper failed", false, false, false)]
    [Arguments(128, "fatal: failed to execute", false, false, false)]
    [Arguments(0, "", false, false, false)]
    [Arguments(129, " ", false, false, false)]
    [Arguments(0, "usage: git [-C <path>] <command>", true, false, false)]
    [Arguments(129, "usage: git [-C <path>] <command>", false, true, false)]
    [Arguments(0, "usage: git [-C <path>] <command>", false, false, true)]
    public async Task Invalid_Root_Usage_Is_Not_Cached_And_Next_Lookup_Can_Recover(
        int exitCode, string output, bool executionFailed, bool timedOut, bool circuitOpen)
    {
        var executor = new RecoveringRootHelpExecutor(new CliCommandResult
        {
            StandardOutput = output,
            StandardError = string.Empty,
            ExitCode = exitCode,
            ExecutionFailed = executionFailed,
            TimedOut = timedOut,
            CircuitOpen = circuitOpen,
        });
        var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        using var scraper = new RootHelpProbe(executor, cache);

        await Assert.That(await scraper.Fetch()).IsNull();
        await Assert.That(cache.TryGet("git", out _)).IsFalse();
        await Assert.That(scraper.UnavailableHelpPaths).IsEquivalentTo(["git"]);

        var recovered = await scraper.Fetch();
        await Assert.That(recovered).Contains("usage: git");
        await Assert.That(cache.TryGet("git", out var cached)).IsTrue();
        await Assert.That(cached).IsEqualTo(recovered);
        await Assert.That(scraper.UnavailableHelpPaths).IsEmpty();
        await Assert.That(await scraper.Fetch()).IsEqualTo(recovered);
        await Assert.That(executor.RootUsageCalls).IsEqualTo(2);
        await Assert.That(executor.CommandListCalls).IsEqualTo(2);
    }

    [Test]
    [Arguments(1, "", false, false, false)]
    [Arguments(128, "fatal: command discovery failed", false, false, false)]
    [Arguments(0, "", false, false, false)]
    [Arguments(0, " ", false, false, false)]
    [Arguments(0, "partial command list", true, false, false)]
    [Arguments(0, "partial command list", false, true, false)]
    [Arguments(0, "partial command list", false, false, true)]
    public async Task Invalid_Root_Discovery_Is_Not_Cached_And_Next_Lookup_Can_Recover(
        int exitCode, string output, bool executionFailed, bool timedOut, bool circuitOpen)
    {
        var executor = new RecoveringRootHelpExecutor(
            Result("usage: git [-C <path>] <command>", exitCode: 129),
            new CliCommandResult
            {
                StandardOutput = output,
                StandardError = string.Empty,
                ExitCode = exitCode,
                ExecutionFailed = executionFailed,
                TimedOut = timedOut,
                CircuitOpen = circuitOpen,
            });
        var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        using var scraper = new RootHelpProbe(executor, cache);

        await Assert.That(await scraper.Fetch()).IsNull();
        await Assert.That(cache.TryGet("git", out _)).IsFalse();
        await Assert.That(scraper.UnavailableHelpPaths).IsEquivalentTo(["git"]);

        var recovered = await scraper.Fetch();
        await Assert.That(recovered).Contains("usage: git");
        await Assert.That(recovered).Contains("Main Porcelain Commands");
        await Assert.That(cache.TryGet("git", out var cached)).IsTrue();
        await Assert.That(cached).IsEqualTo(recovered);
        await Assert.That(scraper.UnavailableHelpPaths).IsEmpty();
        await Assert.That(await scraper.Fetch()).IsEqualTo(recovered);
        await Assert.That(executor.CommandListCalls).IsEqualTo(2);
        await Assert.That(executor.RootUsageCalls).IsEqualTo(2);
    }

    private sealed class RootHelpProbe(ICliCommandExecutor executor, IHelpTextCache cache)
        : GitCliScraper(executor, cache, NullLogger<GitCliScraper>.Instance)
    {
        public Task<string?> Fetch(CancellationToken cancellationToken = default) => GetHelpTextAsync(["git"], cancellationToken);
    }

    private sealed class RecoveringRootHelpExecutor(
        CliCommandResult initialUsage, CliCommandResult? initialDiscovery = null) : ICliCommandExecutor
    {
        public int RootUsageCalls { get; private set; }

        public int CommandListCalls { get; private set; }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            if (arguments == "help -a")
            {
                CommandListCalls++;
                return Task.FromResult(CommandListCalls == 1 && initialDiscovery is not null
                    ? initialDiscovery : Result("Main Porcelain Commands\n   branch                  List branches"));
            }

            if (arguments == "-h")
            {
                RootUsageCalls++;
                return Task.FromResult(RootUsageCalls == 1
                    ? initialUsage : Result("usage: git [-C <path>] <command>", exitCode: 129));
            }

            throw new InvalidOperationException($"Unexpected help command: {arguments}");
        }
    }

    [Test]
    public async Task Empty_Root_Usage_Remains_Unavailable()
    {
        var provenance = new CliScrapeProvenance();
        provenance.Record(["git"], "-h", Result("", exitCode: 129), helpKind: CliHelpKind.Usage);
        await Assert.That(provenance.UnavailableHelpPaths).IsEquivalentTo(["git"]);
    }

    [Test]
    [Arguments(0, 1)]
    [Arguments(128, 129)]
    [Arguments(0, 129)]
    [Arguments(0, 0)]
    public async Task Diagnostics_Preserve_Root_Usage_Separately_From_Command_Discovery(int discoveryExitCode, int usageExitCode)
    {
        var discoveryFails = discoveryExitCode != 0;
        var usageFails = usageExitCode is not (0 or 129);
        var outputDirectory = Path.Combine(Path.GetTempPath(), $"git-root-provenance-{Guid.NewGuid():N}");
        try
        {
            using var scraper = new RootHelpProbe(
                new RecoveringRootHelpExecutor(
                    Result(usageFails ? "wrapper failed" : "usage: git <command>", exitCode: usageExitCode),
                    discoveryFails ? Result("discovery failed", exitCode: discoveryExitCode) : null),
                new StubHelpTextCache());
            var rootHelp = await scraper.Fetch();
            if (discoveryFails || usageFails)
            {
                await Assert.That(rootHelp).IsNull();
            }
            else
            {
                await Assert.That(rootHelp).IsNotNull();
            }
            var coverage = CommandCoverageGuard.Evaluate(
                scraper.CreateToolDefinition(), outputDirectory, approveShrinkage: false,
                unavailableHelpPaths: scraper.UnavailableHelpPaths);
            var path = await scraper.WriteCoverageFailureDiagnosticsAsync(
                outputDirectory, coverage, CancellationToken.None);
            using var diagnostics = JsonDocument.Parse(await File.ReadAllTextAsync(path!));
            var invocations = diagnostics.RootElement.GetProperty("helpInvocations").EnumerateArray().ToArray();

            await Assert.That(invocations.Length).IsEqualTo(2);
            await Assert.That(diagnostics.RootElement.GetProperty("missingHelpPaths").EnumerateArray()
                .Select(value => value.GetString()!)).DoesNotContain("git");
            var help = invocations.Single(invocation => invocation.GetProperty("helpKind").GetString() == "Discovery");
            var usage = invocations.Single(invocation => invocation.GetProperty("helpKind").GetString() == "Usage");
            await Assert.That(help.GetProperty("arguments").GetString()).IsEqualTo("help -a");
            await Assert.That(help.GetProperty("rawHelp").GetString())
                .Contains(discoveryFails ? "discovery failed" : "Main Porcelain Commands");
            await Assert.That(help.GetProperty("exitCode").GetInt32()).IsEqualTo(discoveryExitCode);
            string[] expectedUnavailablePaths = discoveryFails || usageFails ? ["git"] : [];
            await Assert.That(diagnostics.RootElement.GetProperty("unavailableHelpPaths").EnumerateArray()
                .Select(value => value.GetString()!)).IsEquivalentTo(expectedUnavailablePaths);
            await Assert.That(usage.GetProperty("arguments").GetString()).IsEqualTo("-h");
            await Assert.That(usage.GetProperty("rawHelp").GetString())
                .IsEqualTo(usageFails ? "wrapper failed" : "usage: git <command>");
            await Assert.That(usage.GetProperty("exitCode").GetInt32()).IsEqualTo(usageExitCode);
        }
        finally
        {
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, recursive: true);
            }
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(129)]
    public async Task Scrape_Preserves_Branch_When_Root_Switches_Overlap_Local_Switches(int rootUsageExitCode)
    {
        using var logs = LoggerFactory.Create(builder => builder.AddConsole());
        using var scraper = new GitCliScraper(new RootUsageBranchHelpExecutor(rootUsageExitCode), new StubHelpTextCache(), logs.CreateLogger<GitCliScraper>());
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Select(command => command.FullCommand)).IsEquivalentTo(["git branch"]);
        await Assert.That(scraper.UnavailableHelpPaths).IsEmpty();
        var branchOptions = commands.Single().Options
            .Where(option => option.SwitchName is "--delete-merged" or "--forked")
            .ToArray();
        await Assert.That(branchOptions.Select(option => option.SwitchName))
            .IsEquivalentTo(["--delete-merged", "--forked"]);

        foreach (var option in branchOptions)
        {
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    private sealed class RootUsageBranchHelpExecutor(int rootUsageExitCode) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            Task.FromResult(arguments switch
            {
                "help -a" => Result("Main Porcelain Commands\n   branch                  List branches"),
                "-h" => Result("", ReadFixture("root-help.txt"), rootUsageExitCode),
                "branch -h" => Result(ReadFixture("branch-help.txt")),
                _ => Result(""),
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);

        private static string ReadFixture(string name) => File.ReadAllText(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Git", "2.56.0", name));
    }

    [Test]
    public async Task Root_Option_Scraper_Preserves_Repeatable_Value_Shapes()
    {
        const string helpText = """
            usage: git example [<options>]

                --[no-]include <pattern>
                              include matching paths
                              (repeatable)
                --count <n>   number of entries (repeatable)
                --name <name> specify a single name
                -v, --verbose show details (repeatable)
            """;
        using var scraper = new TestGitCliScraper();
        var command = await scraper.Parse(["git", "example"], helpText);
        var options = command!.Options;

        using (Assert.Multiple())
        {
            var include = options.Single(option => option.SwitchName == "--include");
            await Assert.That(include.AcceptsMultipleValues).IsTrue();
            await Assert.That(include.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(include.Description).IsEqualTo("include matching paths (repeatable)");
            var count = options.Single(option => option.SwitchName == "--count");
            await Assert.That(count.AcceptsMultipleValues).IsTrue();
            await Assert.That(count.CSharpType).IsEqualTo("IEnumerable<int>?");
            var noInclude = options.Single(option => option.SwitchName == "--no-include");
            await Assert.That(noInclude.AcceptsMultipleValues).IsFalse();
            await Assert.That(noInclude.CSharpType).IsEqualTo("bool?");
            await Assert.That(noInclude.IsFlag).IsTrue();
            var name = options.Single(option => option.SwitchName == "--name");
            await Assert.That(name.AcceptsMultipleValues).IsFalse();
            await Assert.That(name.CSharpType).IsEqualTo("string?");
            var verbose = options.Single(option => option.SwitchName == "--verbose");
            await Assert.That(verbose.AcceptsMultipleValues).IsFalse();
            await Assert.That(verbose.IsFlag).IsTrue();
            await Assert.That(verbose.CSharpType).IsEqualTo("bool?");
        }
    }

    [Test]
    public async Task Root_Settings_Exclude_Reporting_Actions_And_Internal_Arguments()
    {
        using var scraper = new TestGitCliScraper();
        var options = scraper.ParseGlobals(File.ReadAllText(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Git", "2.56.0", "root-help.txt")));

        await Assert.That(options.Select(option => option.SwitchName)).IsEquivalentTo([
            "-C", "-c", "--config-env", "--exec-path", "--git-dir", "--work-tree", "--namespace",
            "--bare", "--paginate", "--no-pager", "--no-replace-objects", "--no-lazy-fetch",
            "--no-optional-locks", "--no-advice",
        ]);
        await Assert.That(scraper.ParseGlobals("usage: git [--version] [--html-path] [--super-prefix=<path>] <command>"))
            .IsEmpty();
    }

    [Test]
    public async Task Root_Values_Retain_Arity_Repetition_Ordering_And_Secrets()
    {
        using var scraper = new TestGitCliScraper();
        var options = scraper.ParseGlobals("usage: git [-C <path>] [-c <name>=<value>] [--git-dir=<path>] [--config-env=<name>=<envvar>] <command>");
        var directory = options.Single(option => option.SwitchName == "-C");
        var configuration = options.Single(option => option.SwitchName == "-c");
        await Assert.That(directory.CSharpType).IsEqualTo("string[]?");
        await Assert.That(directory.Phase).IsEqualTo(CommandLinePhase.EarlyOperand);
        await Assert.That(configuration.AcceptsMultipleValues).IsTrue();
        await Assert.That(configuration.CSharpType).IsEqualTo("KeyValue[]?");
        await Assert.That(configuration.IsSecret).IsTrue();
        await Assert.That(configuration.ValueSeparator).IsEqualTo(" ");
        await Assert.That(options.Single(option => option.SwitchName == "--git-dir").ValueSeparator).IsEqualTo("=");
        await Assert.That(options.Single(option => option.SwitchName == "--config-env").CSharpType).IsEqualTo("string[]?");
        await Assert.That(options.All(option => !option.IsFlag && option.ValueArity == CliOptionValueArity.Required)).IsTrue();
    }

    [Test]
    public async Task Root_Parsing_Does_Not_Consume_Command_Local_Short_Options()
    {
        using var scraper = new TestGitCliScraper();
        scraper.ParseGlobals("usage: git [-C <path>] [-c <name>=<value>] <command>");
        var command = await scraper.Parse(["git", "apply"], File.ReadAllText(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Git", "2.56.0", "apply-help.txt")));
        var context = command!.Options.Single(option => option.SwitchName == "-C");
        await Assert.That(context.CSharpType).IsEqualTo("int?");
        await Assert.That(context.PropertyName).IsNotEqualTo("ChangeDirectories");
        await Assert.That(scraper.CreateToolDefinition().GenerateCode).IsFalse();
    }
}
