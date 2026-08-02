using System.Collections.ObjectModel;
using CliWrap;
using CliWrap.Buffered;
using ModularPipelines.Engine;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;
using CommandResult = ModularPipelines.Models.CommandResult;

namespace ModularPipelines.Context;

/// <inheritdoc />
internal sealed class CommandLineExecutor(ISecretObfuscator secretObfuscator) : ICommandLineExecutor
{
    private static readonly IReadOnlyDictionary<string, string?> EmptyEnvironmentVariables =
        new ReadOnlyDictionary<string, string?>(new Dictionary<string, string?>());

    /// <inheritdoc />
    public async Task<CommandResult> ExecuteAsync(
        CommandLine commandLine,
        CommandExecutionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var preparedCommand = CliCommandFactory.Create(commandLine.Tool, commandLine.Arguments, options);
        var command = preparedCommand.Command;

        var workingDirectory = Environment.CurrentDirectory;
        var environmentVariables = EmptyEnvironmentVariables;

        if (options?.WorkingDirectory is not null)
        {
            command = command.WithWorkingDirectory(options.WorkingDirectory);
            workingDirectory = options.WorkingDirectory;
        }

        if (options?.EnvironmentVariables is not null)
        {
            environmentVariables = new ReadOnlyDictionary<string, string?>(
                options.EnvironmentVariables.ToDictionary(pair => pair.Key, pair => pair.Value));
        }

        if (options?.CommandLineCredentials is not null)
        {
            command = command.WithCredentials(options.CommandLineCredentials.ToCliWrapCredentials());
        }

        var timeout = options?.ExecutionTimeout ?? CommandExecutionOptions.DefaultExecutionTimeout;
        using var timeoutCts = new CancellationTokenSource(timeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

        try
        {
            var result = await command
                .WithValidation(CommandResultValidation.None)
                .ExecuteBufferedAsync(linkedCts.Token)
                .ConfigureAwait(false);

            return new CommandResult(
                commandInput: secretObfuscator.Obfuscate(preparedCommand.Input, options),
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
