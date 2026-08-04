using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Git.Attributes;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Options;

namespace ModularPipelines.Git.UnitTests;

public class GitChangesTests
{
    [Test]
    public async Task Raw_Git_Command_Runner_Is_A_Public_Extensibility_Contract()
    {
        await Assert.That(typeof(IRawGitCommandRunner).IsPublic).IsTrue();
    }

    [Test]
    public async Task Matches_Changed_Paths_And_Caches_Git_Diff()
    {
        var runner = new RecordingGitCommandRunner(
            "0123456789abcdef",
            "src/MyService/Program.cs\0docs/readme.md\0");
        var changes = CreateChanges(runner);

        var sourceChanged = await changes.HasChangesAsync(["src/MyService/**"]);
        var testsChanged = await changes.HasChangesAsync(["test/**"]);

        await Assert.That(sourceChanged).IsTrue();
        await Assert.That(testsChanged).IsFalse();
        await Assert.That(runner.Commands).Count().IsEqualTo(3);
        await Assert.That(runner.Commands[0].OfType<string>())
            .IsEquivalentTo(["merge-base", "origin/main", "HEAD"]);
        await Assert.That(runner.Commands[1].OfType<string>()).IsEquivalentTo(
            ["diff", "--name-only", "--no-renames", "-z", "0123456789abcdef", "--"]);
        await Assert.That(runner.Commands[2].OfType<string>()).IsEquivalentTo(
            [
                "ls-files", "--others", "--exclude-standard", "--full-name", "-z", "--", ":(top)"
            ]);
    }

    [Test]
    public async Task Uses_A_Separate_Cache_For_Each_Base_Reference()
    {
        var runner = new RecordingGitCommandRunner(
            "main-base",
            "src/main.cs\0",
            "",
            "release-base",
            "src/release.cs\0");
        var changes = CreateChanges(runner);

        await changes.HasChangesAsync(["src/**"]);
        await changes.HasChangesAsync(["src/**"], "origin/release");

        await Assert.That(runner.Commands).Count().IsEqualTo(5);
        await Assert.That(runner.Commands[3].OfType<string>())
            .IsEquivalentTo(["merge-base", "origin/release", "HEAD"]);
        await Assert.That(runner.Commands.Count(command => command[0] is "ls-files")).IsEqualTo(1);
    }

    [Test]
    public async Task Normalizes_Windows_Style_Glob_Separators()
    {
        var runner = new RecordingGitCommandRunner(
            "merge-base",
            "src/MyService/Program.cs\0");
        var changes = CreateChanges(runner);

        var hasChanges = await changes.HasChangesAsync([@"src\MyService\**"]);

        await Assert.That(hasChanges).IsTrue();
    }

    [Test]
    public async Task Matches_Untracked_Files_Using_Repository_Root_Paths()
    {
        var runner = new RecordingGitCommandRunner(
            "merge-base",
            "",
            "src/MyService/NewFile.cs\0");
        var changes = CreateChanges(runner);

        var hasChanges = await changes.HasChangesAsync(["src/MyService/**"]);

        await Assert.That(hasChanges).IsTrue();
        await Assert.That(runner.Commands[2].OfType<string>()).IsEquivalentTo(
            [
                "ls-files", "--others", "--exclude-standard", "--full-name", "-z", "--", ":(top)"
            ]);
    }

    [Test]
    public async Task Literal_Directory_Pattern_Matches_Descendants()
    {
        var runner = new RecordingGitCommandRunner(
            "merge-base",
            "src/MyService/Program.cs\0");
        var changes = CreateChanges(runner);

        var hasChanges = await changes.HasChangesAsync(["src/MyService"]);

        await Assert.That(hasChanges).IsTrue();
    }

    [Test]
    public async Task Literal_Directory_Pattern_Requires_Path_Boundary()
    {
        var runner = new RecordingGitCommandRunner(
            "merge-base",
            "src/MyService.Tests/Program.cs\0");
        var changes = CreateChanges(runner);

        var hasChanges = await changes.HasChangesAsync(["src/MyService"]);

        await Assert.That(hasChanges).IsFalse();
    }

    [Test]
    public async Task Preserves_Literal_Backslashes_In_Git_Paths()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var runner = new RecordingGitCommandRunner(
            "merge-base",
            "src\\MyService\\Program.cs\0");
        var changes = CreateChanges(runner);

        var exactMatch = await changes.HasChangesAsync(["src/MyService/Program.cs"]);
        var childMatch = await changes.HasChangesAsync(["src/**"]);
        var rootNameMatch = await changes.HasChangesAsync(["src*"]);
        var repositoryMatch = await changes.HasChangesAsync(["**"]);

        using (Assert.Multiple())
        {
            await Assert.That(exactMatch).IsFalse();
            await Assert.That(childMatch).IsFalse();
            await Assert.That(rootNameMatch).IsTrue();
            await Assert.That(repositoryMatch).IsTrue();
        }
    }

    [Test]
    public async Task Preserves_Whitespace_In_Null_Delimited_Paths_And_Patterns()
    {
        var runner = new RecordingGitCommandRunner(
            "merge-base\n",
            " leading-directory/file.txt \0");
        var changes = CreateChanges(runner);

        var hasChanges = await changes.HasChangesAsync([" leading-directory/file.txt "]);

        await Assert.That(hasChanges).IsTrue();
        await Assert.That(runner.RawCommands).Count().IsEqualTo(2);
        await Assert.That(runner.Commands[0].OfType<string>()).Contains("merge-base");
    }

    [Test]
    public async Task Captures_The_Complete_Null_Delimited_Diff()
    {
        var runner = new RecordingGitCommandRunner(
            "merge-base",
            "src/MyService/Program.cs\0");
        var changes = CreateChanges(runner);

        await changes.HasChangesAsync(["src/**"]);

        await Assert.That(runner.ExecutionOptions[1]?.MaxCapturedOutputLength).IsEqualTo(0);
        await Assert.That(runner.ExecutionOptions[2]?.MaxCapturedOutputLength).IsEqualTo(0);
    }

    [Test]
    public async Task Includes_Source_And_Destination_Paths_For_Renames()
    {
        var runner = new RecordingGitCommandRunner(
            "merge-base",
            "src/A/file.cs\0src/B/file.cs\0");
        var changes = CreateChanges(runner);

        var sourceChanged = await changes.HasChangesAsync(["src/A/**"]);

        await Assert.That(sourceChanged).IsTrue();
        await Assert.That(runner.Commands[1].OfType<string>()).Contains("--no-renames");
    }

    [Test]
    public async Task Includes_Staged_And_Unstaged_Changes_After_The_Merge_Base()
    {
        var runner = new RecordingGitCommandRunner(
            "merge-base",
            "src/committed.cs\0src/staged.cs\0src/unstaged.cs\0");
        var changes = CreateChanges(runner);

        var stagedChanged = await changes.HasChangesAsync(["src/staged.cs"]);
        var unstagedChanged = await changes.HasChangesAsync(["src/unstaged.cs"]);

        using (Assert.Multiple())
        {
            await Assert.That(stagedChanged).IsTrue();
            await Assert.That(unstagedChanged).IsTrue();
            await Assert.That(runner.Commands[1].OfType<string>()).DoesNotContain("HEAD");
        }
    }

    [Test]
    public async Task Unavailable_Base_Conservatively_Reports_Changes_And_Is_Cached()
    {
        var runner = new RecordingGitCommandRunner { FailCommandsOrNull = true };
        var logger = new RecordingLogger<GitChanges>();
        var changes = CreateChanges(runner, logger);

        var firstCheck = await changes.HasChangesAsync(["src/**"]);
        var secondCheck = await changes.HasChangesAsync(["docs/**"]);

        using (Assert.Multiple())
        {
            await Assert.That(firstCheck).IsTrue();
            await Assert.That(secondCheck).IsTrue();
            await Assert.That(runner.Commands).Count().IsEqualTo(1);
            await Assert.That(logger.Messages).Count().IsEqualTo(1);
            await Assert.That(logger.Messages[0]).Contains("origin/main");
        }
    }

    [Test]
    public async Task Shares_Changed_Path_Cache_Across_Module_Scopes()
    {
        var runner = new RecordingGitCommandRunner(
            "merge-base",
            "src/MyService/Program.cs\0");
        var services = new ServiceCollection();
        services.AddSingleton<IGitCommandRunner>(runner);
        services.RegisterGitContext();
        await using var provider = services.BuildServiceProvider();

        await using (var firstScope = provider.CreateAsyncScope())
        {
            await firstScope.ServiceProvider.GetRequiredService<IGitChanges>()
                .HasChangesAsync(["src/**"]);
        }

        await using (var secondScope = provider.CreateAsyncScope())
        {
            await secondScope.ServiceProvider.GetRequiredService<IGitChanges>()
                .HasChangesAsync(["docs/**"]);
        }

        await Assert.That(runner.Commands).Count().IsEqualTo(3);
    }

    [Test]
    public async Task Rejects_An_Empty_Pattern_Set()
    {
        var changes = CreateChanges(new RecordingGitCommandRunner());

        async Task Check() => await changes.HasChangesAsync([]);

        await Assert.That(Check).Throws<ArgumentException>();
    }

    [Test]
    public async Task RunIfChanged_Exposes_Globs_And_Base_Reference()
    {
        var attribute = new RunIfChangedAttribute("src/**", "test/**")
        {
            Base = "origin/release",
        };

        await Assert.That(attribute.PathPatterns).IsEquivalentTo(["src/**", "test/**"]);
        await Assert.That(attribute.Base).IsEqualTo("origin/release");
        await Assert.That(attribute.ConditionNames)
            .IsEqualTo("RunIfChangedAttribute(src/**, test/**; Base=origin/release)");
    }

    private static IGitChanges CreateChanges(
        IGitCommandRunner runner,
        ILogger<GitChanges>? logger = null)
    {
        var services = new ServiceCollection();
        services.AddSingleton(runner);
        services.AddSingleton(logger ?? NullLogger<GitChanges>.Instance);
        services.RegisterGitContext();
        return services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<IGitChanges>();
    }

    private sealed class RecordingGitCommandRunner(params string[] outputs)
        : IGitCommandRunner, IRawGitCommandRunner
    {
        private readonly Queue<string> _outputs = new(outputs);

        public List<string?[]> Commands { get; } = [];

        public List<string?[]> RawCommands { get; } = [];

        public List<CommandExecutionOptions?> ExecutionOptions { get; } = [];

        public bool FailCommandsOrNull { get; init; }

        public Task<string> RunCommands(CommandExecutionOptions? commandEnvironmentOptions, params string?[] commands)
        {
            Commands.Add(commands);
            ExecutionOptions.Add(commandEnvironmentOptions);
            return Task.FromResult(NextOutput().Trim());
        }

        Task<string> IRawGitCommandRunner.RunCommandsUntrimmed(
            CommandExecutionOptions? commandEnvironmentOptions,
            CancellationToken cancellationToken,
            params string?[] commands)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Commands.Add(commands);
            RawCommands.Add(commands);
            ExecutionOptions.Add(commandEnvironmentOptions);
            return Task.FromResult(NextOutput());
        }

        public Task<string?> RunCommandsOrNull(
            CommandExecutionOptions? commandEnvironmentOptions,
            params string?[] commands)
        {
            Commands.Add(commands);
            ExecutionOptions.Add(commandEnvironmentOptions);
            return Task.FromResult<string?>(FailCommandsOrNull ? null : NextOutput().Trim());
        }

        private string NextOutput() => _outputs.TryDequeue(out var output) ? output : string.Empty;
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (logLevel == LogLevel.Warning)
            {
                Messages.Add(formatter(state, exception));
            }
        }
    }
}
