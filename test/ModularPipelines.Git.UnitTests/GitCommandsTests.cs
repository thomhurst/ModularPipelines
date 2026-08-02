using ModularPipelines.Options;

namespace ModularPipelines.Git.UnitTests;

public class GitCommandsTests
{
    [Test]
    public async Task Commits_Uses_Unquoted_Pretty_Format()
    {
        var runner = new RecordingGitCommandRunner(string.Join(
            "%\n%",
            "Author",
            "author@example.com",
            "2026-07-31T13:00:45Z",
            "Committer",
            "committer@example.com",
            "2026-07-31T12:15:30Z",
            "0123456789abcdef0123456789abcdef01234567",
            "0123456",
            "Subject",
            string.Empty));
        var commands = new GitCommands(null!, runner, new GitCommitMapper());

        await foreach (var _ in commands.CommitsAsync())
        {
            break;
        }

        var formatArgument = runner.Commands!.Single(command => command?.StartsWith("--format=", StringComparison.Ordinal) == true);
        await Assert.That(formatArgument).StartsWith("--format=%aN");
        await Assert.That(formatArgument).DoesNotContain("'");
    }

    private sealed class RecordingGitCommandRunner(string output) : IGitCommandRunner
    {
        public string?[]? Commands { get; private set; }

        public Task<string> RunCommands(CommandExecutionOptions? commandEnvironmentOptions, params string?[] commands)
        {
            throw new NotSupportedException();
        }

        public Task<string?> RunCommandsOrNull(CommandExecutionOptions? commandEnvironmentOptions, params string?[] commands)
        {
            Commands = commands;
            return Task.FromResult<string?>(output);
        }
    }
}
