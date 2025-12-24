using System.Runtime.CompilerServices;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

internal class GitInformation : IGitInformation, IInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public GitInformation(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Folder Root { get; private set; } = null!;
    public string BranchName { get; private set; } = "";
    public string DefaultBranchName { get; private set; } = "";
    public string LastCommitSha { get; private set; } = "";
    public string LastCommitShortSha { get; private set; } = "";
    public string Tag { get; private set; } = "";
    public int CommitsOnBranch { get; private set; }
    public DateTimeOffset LastCommitDateTime { get; private set; }
    public GitCommit? PreviousCommit { get; private set; }

    public async Task InitializeAsync()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<GitInformation>>();
        var command = scope.ServiceProvider.GetRequiredService<ICommand>();
        var gitCommandRunner = scope.ServiceProvider.GetRequiredService<GitCommandRunner>();
        var gitCommitMapper = scope.ServiceProvider.GetRequiredService<IGitCommitMapper>();

        await VerifyGitAvailable(command);

        var tasks = new List<Task>();

        Async(async () =>
        {
            var root = await GetOutput(command, logger, new GitRevParseOptions { ShowToplevel = true });
            Root = root != null ? new Folder(root) : throw new InvalidOperationException("Not a git repository");
        });

        Async(async () => BranchName = await GetOutput(command, logger, new GitBranchOptions { ShowCurrent = true }) ?? "");

        Async(async () => DefaultBranchName = await GetDefaultBranchName(command, logger) ?? "");

        Async(async () => LastCommitSha = await GetOutput(command, logger, new GitRevParseOptions { Arguments = ["HEAD"] }) ?? "");

        Async(async () => LastCommitShortSha = await GetOutput(command, logger, new GitRevParseOptions { Short = true, Arguments = ["HEAD"] }) ?? "");

        Async(async () => Tag = await GetOutput(command, logger, new GitDescribeOptions { Tags = true }) ?? "");

        Async(async () =>
        {
            var countStr = await GetOutput(command, logger, new GitRevListOptions { Count = true, Arguments = ["HEAD"] });
            CommitsOnBranch = int.TryParse(countStr, out var count) ? count : 0;
        });

        Async(async () =>
        {
            var timestampStr = await GetOutput(command, logger, new GitLogOptions { Format = "%at", Arguments = ["-1"] });
            LastCommitDateTime = long.TryParse(timestampStr, out var timestamp)
                ? DateTimeOffset.FromUnixTimeSeconds(timestamp)
                : DateTimeOffset.MinValue;
        });

        Async(async () => PreviousCommit = await GetPreviousCommit(gitCommandRunner, gitCommitMapper));

        await Task.WhenAll(tasks);
        return;

        void Async(Func<Task> task) => tasks.Add(task());
    }

    private static async Task VerifyGitAvailable(ICommand command)
    {
        try
        {
            await command.ExecuteCommandLineTool(new CommandLineToolOptions("git")
            {
                Arguments = ["version"],
                LogSettings = CommandLoggingOptions.Silent,
            });
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("Git is not available. Ensure git is installed and in PATH.", e);
        }
    }

    private static async Task<string?> GetDefaultBranchName(ICommand command, ILogger logger)
    {
        try
        {
            var output = await GetOutput(command, logger, new GitRemoteOptions { Arguments = ["show", "origin"] });
            if (output == null)
            {
                return null;
            }

            return output.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(x => x.StartsWith("HEAD branch:"))
                ?.Split("HEAD branch: ")[1];
        }
        catch
        {
            var output = await GetOutput(command, logger, new GitRevParseOptions
            {
                Arguments = ["origin/HEAD"],
                AbbrevRef = true,
                ThrowOnNonZeroExitCode = false,
            });

            return output?.Replace("origin/", string.Empty);
        }
    }

    private static async Task<string?> GetOutput(ICommand command, ILogger logger, GitOptions gitOptions)
    {
        try
        {
            var result = await command.ExecuteCommandLineTool(gitOptions with
            {
                LogSettings = logger.IsEnabled(LogLevel.Debug) ? CommandLoggingOptions.Diagnostic : CommandLoggingOptions.Silent,
            });
            return result.StandardOutput.Trim();
        }
        catch (Exception exception)
        {
            logger.LogDebug(exception, "Error running Git command");
            return null;
        }
    }

    private static async Task<GitCommit?> GetPreviousCommit(GitCommandRunner gitCommandRunner, IGitCommitMapper gitCommitMapper)
    {
        var output = await gitCommandRunner.RunCommandsOrNull(null, "log", "--skip=0", "-1",
            $"--format='%aN {GitConstants.GitEscapedLineSeparator} %aE {GitConstants.GitEscapedLineSeparator} %aI {GitConstants.GitEscapedLineSeparator} %cN {GitConstants.GitEscapedLineSeparator} %cE {GitConstants.GitEscapedLineSeparator} %cI {GitConstants.GitEscapedLineSeparator} %H {GitConstants.GitEscapedLineSeparator} %h {GitConstants.GitEscapedLineSeparator} %s {GitConstants.GitEscapedLineSeparator} %b'");

        return string.IsNullOrWhiteSpace(output) ? null : gitCommitMapper.Map(output);
    }

    public IAsyncEnumerable<GitCommit> Commits(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return Commits(null, options, cancellationToken);
    }

    public async IAsyncEnumerable<GitCommit> Commits(string? branch, GitOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var gitCommandRunner = _serviceProvider.GetRequiredService<GitCommandRunner>();
        var gitCommitMapper = _serviceProvider.GetRequiredService<IGitCommitMapper>();

        var index = 0;
        while (true)
        {
            var output = await gitCommandRunner.RunCommandsOrNull(options, "log", branch, $"--skip={index - 1}", "-1", $"--format='%aN {GitConstants.GitEscapedLineSeparator} %aE {GitConstants.GitEscapedLineSeparator} %aI {GitConstants.GitEscapedLineSeparator} %cN {GitConstants.GitEscapedLineSeparator} %cE {GitConstants.GitEscapedLineSeparator} %cI {GitConstants.GitEscapedLineSeparator} %H {GitConstants.GitEscapedLineSeparator} %h {GitConstants.GitEscapedLineSeparator} %s {GitConstants.GitEscapedLineSeparator} %B'");

            index++;

            if (string.IsNullOrWhiteSpace(output) || cancellationToken.IsCancellationRequested)
            {
                yield break;
            }

            yield return gitCommitMapper.Map(output);
        }
    }
}
