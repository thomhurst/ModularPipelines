using ModularPipelines.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class GitCliScraperTests
{
    [Test]
    [Arguments(1, "wrapper failed", false)]
    [Arguments(128, "fatal: failed to execute", false)]
    [Arguments(0, "", false)]
    [Arguments(129, " ", false)]
    [Arguments(0, "usage: git [-C <path>] <command>", true)]
    public async Task Invalid_Root_Usage_Is_Not_Cached_And_Next_Lookup_Can_Recover(
        int exitCode, string output, bool executionFailed)
    {
        var executor = new RecoveringRootHelpExecutor(new CliCommandResult
        {
            StandardOutput = output,
            StandardError = string.Empty,
            ExitCode = exitCode,
            ExecutionFailed = executionFailed,
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

    private sealed class RootHelpProbe(ICliCommandExecutor executor, IHelpTextCache cache)
        : GitCliScraper(executor, cache, NullLogger<GitCliScraper>.Instance)
    {
        public Task<string?> Fetch() => GetHelpTextAsync(["git"], CancellationToken.None);
    }

    private sealed class RecoveringRootHelpExecutor(CliCommandResult initialUsage) : ICliCommandExecutor
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
                return Task.FromResult(Result("Main Porcelain Commands\n   branch                  List branches"));
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
    [Arguments(0)]
    [Arguments(129)]
    public async Task Scrape_Preserves_Branch_When_Root_Switches_Overlap_Local_Switches(int rootUsageExitCode)
    {
        using var logs = LoggerFactory.Create(builder => builder.AddConsole());
        using var scraper = new GitCliScraper(new BranchHelpExecutor(rootUsageExitCode), new StubHelpTextCache(), logs.CreateLogger<GitCliScraper>());
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

    private sealed class BranchHelpExecutor(int rootUsageExitCode) : ICliCommandExecutor
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
