using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class GitCliScraperTests
{
    [Test]
    public async Task Tool_Definition_Preserves_The_Hand_Written_Grouped_Facade()
    {
        var scraper = new GitCliScraper(
            new StubExecutor(),
            new StubHelpTextCache(),
            NullLogger<GitCliScraper>.Instance);

        var tool = scraper.CreateToolDefinition();

        await Assert.That(tool.GenerateCode).IsFalse();
        await Assert.That(tool.DocumentationOutputDirectory).IsNull();
    }

    [Test]
    public async Task ScrapeAsync_Discovers_Generic_Groups_Without_Repeating_Parent_Help()
    {
        var executor = new GroupedHelpExecutor();
        string helpRepository;
        using (var scraper = new GitCliScraper(
                   executor,
                   new StubHelpTextCache(),
                   NullLogger<GitCliScraper>.Instance))
        {
            var commands = new List<CliCommandDefinition>();
            await foreach (var command in scraper.ScrapeAsync())
            {
                commands.Add(command);
            }

            using (Assert.Multiple())
            {
                await Assert.That(commands.Select(command => command.FullCommand))
                    .IsEquivalentTo(["git stash", "git stash pop", "git status"]);
                await Assert.That(executor.StashHelpInvocations).IsEqualTo(1);
                await Assert.That(executor.StatusHelpInvocations).IsEqualTo(1);
                await Assert.That(executor.NestedHelpWorkingDirectory).IsNotNull();
            }

            helpRepository = executor.NestedHelpWorkingDirectory!;
        }

        await Assert.That(Directory.Exists(helpRepository)).IsFalse();
    }

    [Test]
    public async Task ScrapeAsync_Uses_Stdout_Help_When_Stderr_Is_Empty()
    {
        var scraper = new GitCliScraper(
            new StdoutHelpExecutor(),
            new StubHelpTextCache(),
            NullLogger<GitCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        using (Assert.Multiple())
        {
            await Assert.That(commands).HasSingleItem();
            await Assert.That(commands[0].FullCommand).IsEqualTo("git switch");
            await Assert.That(commands[0].Options.Single().SwitchName).IsEqualTo("--force");
        }
    }

    [Test]
    public async Task Dispose_Waits_For_InFlight_Repository_Initialization()
    {
        var executor = new BlockingInitExecutor();
        var scraper = new GitCliScraper(
            executor,
            new StubHelpTextCache(),
            NullLogger<GitCliScraper>.Instance);
        using var cancellationTokenSource = new CancellationTokenSource();

        var scrapeTask = DrainAsync(scraper, cancellationTokenSource.Token);
        await executor.InitializationStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));
        cancellationTokenSource.Cancel();
        var disposalStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var disposeTask = Task.Run(() =>
        {
            disposalStarted.SetResult();
            scraper.Dispose();
        });
        await disposalStarted.Task;
        await Task.Delay(50);

        await Assert.That(disposeTask.IsCompleted).IsFalse();
        executor.AllowInitialization.SetResult();
        await Assert.That(async () => await scrapeTask).Throws<OperationCanceledException>();
        await disposeTask;

        using (Assert.Multiple())
        {
            await Assert.That(executor.RepositoryExistedAfterInitialization).IsTrue();
            await Assert.That(Directory.Exists(executor.RepositoryDirectory!)).IsFalse();
        }
    }

    [Test]
    public async Task ScrapeAsync_Throws_After_Disposal()
    {
        var scraper = new GitCliScraper(
            new StubExecutor(),
            new StubHelpTextCache(),
            NullLogger<GitCliScraper>.Instance);
        scraper.Dispose();

        await Assert.That(async () => await DrainAsync(scraper, CancellationToken.None))
            .Throws<ObjectDisposedException>();
    }

    [Test]
    public async Task ExtractSubcommands_Skips_Global_Options_Before_Command_Name()
    {
        const string help = """
                            usage: git remote [-v | --verbose]
                               or: git remote [-v | --verbose] show [-n] <name>
                               or: git remote get-url [--push] [--all] <name>
                               or: git remote set-url [--push] <name> <newurl>
                            """;

        var subcommands = GitCliScraper.ExtractSubcommands("remote", help);

        await Assert.That(subcommands).IsEquivalentTo(["get-url", "set-url", "show"]);
    }

    [Test]
    public async Task ExtractSubcommands_Recognizes_Optional_Default_Command()
    {
        const string help = """
                            or: git stash [push] [-p | --patch]
                            or: git stash pop [--index]
                            """;

        var subcommands = GitCliScraper.ExtractSubcommands("stash", help);

        await Assert.That(subcommands).IsEquivalentTo(["pop", "push"]);
    }

    [Test]
    public async Task ExtractSubcommands_Rejects_Optional_Operand_Labels()
    {
        const string help = "usage: git request-pull [options] start url [end]";

        var subcommands = GitCliScraper.ExtractSubcommands("request-pull", help);

        await Assert.That(subcommands).IsEmpty();
    }

    [Test]
    public async Task ExtractSubcommands_Handles_Nested_Option_Brackets()
    {
        const string help = """
                            usage: git notes [--ref <notes-ref>] [list [<object>]]
                               or: git notes [--ref <notes-ref>] add [-f] [--[no-]stripspace]
                               or: git notes [--ref <notes-ref>] append [--[no-]stripspace]
                            """;

        var subcommands = GitCliScraper.ExtractSubcommands("notes", help);

        await Assert.That(subcommands).IsEquivalentTo(["add", "append", "list"]);
        await Assert.That(subcommands.Contains("stripspace")).IsFalse();
    }

    [Test]
    public async Task ExtractSubcommands_Allows_Alternatives_After_Bare_Command()
    {
        const string help =
            "or: git submodule [--quiet] add [-f | --force] <repository>";

        var subcommands = GitCliScraper.ExtractSubcommands("submodule", help);

        await Assert.That(subcommands).IsEquivalentTo(["add"]);
    }

    [Test]
    public async Task Parses_Negatable_Option_Without_Colliding_With_Operand()
    {
        const string helpText = """
            usage: git merge [<options>] [<commit>...]

                --[no-]commit       perform a commit if the merge succeeds (default)
            """;
        using var scraper = new TestGitCliScraper();
        var command = await scraper.Parse(["git", "merge"], helpText);
        var resolved = InheritedPropertyCollisionResolver.Resolve(
                scraper.CreateToolDefinition() with { Commands = [command!] })
            .Commands.Single();

        using (Assert.Multiple())
        {
            var commit = resolved.Options.Single(option => option.SwitchName == "--commit");
            var noCommit = resolved.Options.Single(option => option.SwitchName == "--no-commit");
            await Assert.That(commit.PropertyName).IsEqualTo("Commit");
            await Assert.That(commit.IsFlag).IsTrue();
            await Assert.That(noCommit.PropertyName).IsEqualTo("NoCommit");
            await Assert.That(noCommit.IsFlag).IsTrue();
            await Assert.That(resolved.PositionalArguments.Single().PropertyName)
                .IsEqualTo("CommitArgument");
            await Assert.That(resolved.PositionalArguments.Single().IsVariadic).IsTrue();
        }
    }

    [Test]
    public async Task Parses_Value_Taking_And_Alternative_Valued_Negatable_Options()
    {
        const string helpText = """
            usage: git fetch [<options>]

                --[no-]upload-pack <path>                       path to upload pack
                --[no-]recurse-submodules[=<on-demand>]         control recursive fetching
                --[no-]signed[=(yes|no|if-asked)]               GPG sign the request
            """;
        using var scraper = new TestGitCliScraper();
        var command = await scraper.Parse(["git", "fetch"], helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Single(option => option.SwitchName == "--upload-pack").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--recurse-submodules").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.SwitchName == "--signed").IsFlag)
                .IsFalse();
            await Assert.That(command.Options
                    .Where(option => option.SwitchName is "--no-upload-pack" or "--no-recurse-submodules" or "--no-signed")
                    .All(option => option.IsFlag && option.CSharpType == "bool?"))
                .IsTrue();
        }
    }

    private static CliCommandResult Result(
        string standardOutput,
        string standardError = "",
        int exitCode = 0) =>
        new()
        {
            StandardOutput = standardOutput,
            StandardError = standardError,
            ExitCode = exitCode,
        };

    private static async Task DrainAsync(
        GitCliScraper scraper,
        CancellationToken cancellationToken)
    {
        await foreach (var _ in scraper.ScrapeAsync(cancellationToken))
        {
        }
    }

    private sealed class StubExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            throw new NotSupportedException();

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();
    }

    private sealed class TestGitCliScraper : GitCliScraper
    {
        public TestGitCliScraper()
            : base(
                new StubExecutor(),
                new StubHelpTextCache(),
                NullLogger<GitCliScraper>.Instance)
        {
        }

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                UsageSynopsisParser.Parse(helpText, commandPath),
                CancellationToken.None);
    }

    private sealed class StdoutHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(arguments switch
            {
                "help -a" => Result(
                    "Main Porcelain Commands\n   switch                  Switch branches"),
                "switch -h" => Result(
                    "usage: git switch [<options>] [<branch>]\n    -f, --force            force checkout",
                    exitCode: 129),
                _ => Result(string.Empty, "unexpected invocation", exitCode: 1),
            });

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class GroupedHelpExecutor : ICliCommandExecutor
    {
        public int StashHelpInvocations { get; private set; }

        public int StatusHelpInvocations { get; private set; }

        public string? NestedHelpWorkingDirectory { get; private set; }

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            if (arguments == "stash -h")
            {
                StashHelpInvocations++;
            }
            else if (arguments == "status -h")
            {
                StatusHelpInvocations++;
            }
            else if (arguments == "stash pop -h")
            {
                NestedHelpWorkingDirectory = workingDirectory;
            }

            return Task.FromResult(arguments switch
            {
                "init --quiet" => Result(string.Empty),
                "help -a" => Result(
                    "Main Porcelain Commands\n"
                    + "   stash                   Stash changes\n"
                    + "   status                  Show status"),
                "stash -h" => Result(
                    "usage: git stash\n   or: git stash pop [--index]"),
                "stash pop -h" => Result("usage: git stash pop [--index]"),
                "status -h" => Result("usage: git status [<options>]"),
                _ => Result(string.Empty, "unexpected invocation", exitCode: 1),
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class BlockingInitExecutor : ICliCommandExecutor
    {
        public TaskCompletionSource InitializationStarted { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource AllowInitialization { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public string? RepositoryDirectory { get; private set; }

        public bool RepositoryExistedAfterInitialization { get; private set; }

        public async Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            if (arguments == "init --quiet")
            {
                RepositoryDirectory = workingDirectory;
                InitializationStarted.SetResult();
                await AllowInitialization.Task;
                RepositoryExistedAfterInitialization = Directory.Exists(workingDirectory);
                return Result(string.Empty);
            }

            cancellationToken.ThrowIfCancellationRequested();
            return arguments switch
            {
                "help -a" => Result("Main Porcelain Commands\n   stash                   Stash changes"),
                "stash -h" => Result("usage: git stash\n   or: git stash pop [--index]"),
                "stash pop -h" => Result("usage: git stash pop [--index]"),
                _ => Result(string.Empty, "unexpected invocation", exitCode: 1),
            };
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class StubHelpTextCache : IHelpTextCache
    {
        private readonly Dictionary<string, string> _entries =
            new(StringComparer.OrdinalIgnoreCase);

        public bool TryGet(string cacheKey, out string? helpText) =>
            _entries.TryGetValue(cacheKey, out helpText);

        public void Set(string cacheKey, string helpText) =>
            _entries[cacheKey] = helpText;

        public void Clear() => _entries.Clear();

        public CacheStatistics GetStatistics() => new()
        {
            EntryCount = _entries.Count,
        };
    }
}
