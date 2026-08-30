using System.Collections.Concurrent;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;

namespace ModularPipelines.Testing;

internal sealed class RecordingCommandInterceptor : ICommandInterceptor
{
    private readonly ConcurrentQueue<(long Sequence, RecordedCommand Command)> _commands = new();
    private Func<CommandInvocation, CancellationToken, ValueTask<CommandResult>> _handler =
        static (_, _) => ValueTask.FromResult(CommandResult.Ok());
    private long _nextSequence;

    public IReadOnlyList<RecordedCommand> Commands =>
        [.. _commands.OrderBy(record => record.Sequence).Select(record => record.Command)];

    public void SetHandler(Func<CommandInvocation, CancellationToken, ValueTask<CommandResult>> handler)
    {
        _handler = handler;
    }

    public async ValueTask<CommandResult?> InterceptAsync(
        CommandInvocation invocation,
        CancellationToken cancellationToken = default)
    {
        var sequence = Interlocked.Increment(ref _nextSequence);
        var result = await _handler(invocation, cancellationToken).ConfigureAwait(false);
        var effectiveResult = result with
        {
            CommandInput = invocation.CommandInput,
            WorkingDirectory = invocation.WorkingDirectory,
            EnvironmentVariables = invocation.EnvironmentVariables,
        };
        _commands.Enqueue((sequence, new RecordedCommand(invocation, effectiveResult)));
        return result;
    }
}
