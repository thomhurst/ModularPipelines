using System.Collections.ObjectModel;
using CliWrap;
using CliWrap.Buffered;
using ModularPipelines.Models;
using ModularPipelines.Options;
using CommandResult = ModularPipelines.Models.CommandResult;

namespace ModularPipelines.Context;

/// <inheritdoc />
internal sealed class CommandLineExecutor : ICommandLineExecutor
{
    private static readonly IReadOnlyDictionary<string, string?> EmptyEnvironmentVariables =
        new ReadOnlyDictionary<string, string?>(new Dictionary<string, string?>());

    /// <inheritdoc />
    public async Task<CommandResult> ExecuteAsync(
        CommandLine commandLine,
        CommandExecutionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var command = Cli.Wrap(commandLine.Tool)
            .WithArguments(commandLine.Arguments);

        var workingDirectory = Environment.CurrentDirectory;
        IReadOnlyDictionary<string, string?> environmentVariables = EmptyEnvironmentVariables;

        if (options?.WorkingDirectory is not null)
        {
            command = command.WithWorkingDirectory(options.WorkingDirectory);
            workingDirectory = options.WorkingDirectory;
        }

        if (options?.EnvironmentVariables is not null)
        {
            command = command.WithEnvironmentVariables(new ReadOnlyDictionary<string, string?>(options.EnvironmentVariables));
            environmentVariables = new ReadOnlyDictionary<string, string?>(options.EnvironmentVariables);
        }

        var timeout = options?.ExecutionTimeout ?? TimeSpan.FromMinutes(30);
        using var timeoutCts = new CancellationTokenSource(timeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

        try
        {
            var result = await command
                .WithValidation(CommandResultValidation.None)
                .ExecuteBufferedAsync(linkedCts.Token)
                .ConfigureAwait(false);

            return new CommandResult(
                commandInput: commandLine.ToString(),
                workingDirectory: workingDirectory,
                standardOutput: result.StandardOutput,
                standardError: result.StandardError,
                environmentVariables: environmentVariables,
                startTime: result.StartTime,
                endTime: result.ExitTime,
                duration: result.RunTime,
                exitCode: result.ExitCode);
        }
        catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
        {
            throw new TimeoutException($"Command '{commandLine}' timed out after {timeout}");
        }
    }
}
