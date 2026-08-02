using ModularPipelines.Options;

namespace ModularPipelines.Git;

internal interface IRawGitCommandRunner
{
    Task<string> RunCommandsUntrimmed(
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken,
        params string?[] commands);
}
