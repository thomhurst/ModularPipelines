using System.Collections.Concurrent;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;

namespace ModularPipelines.Testing;

internal sealed class RecordingCommandInterceptor : ICommandInterceptor
{
    private readonly ConcurrentQueue<RecordedCommand> _commands = new();
    private Func<CommandInvocation, CancellationToken, ValueTask<CommandResult>> _handler =
        static (_, _) => ValueTask.FromResult(CommandResult.Ok());

    public IReadOnlyList<RecordedCommand> Commands => [.. _commands];

    public void SetHandler(Func<CommandInvocation, CancellationToken, ValueTask<CommandResult>> handler)
    {
        _handler = handler;
    }

    public async ValueTask<CommandResult?> InterceptAsync(
        CommandInvocation invocation,
        CancellationToken cancellationToken = default)
    {
        var result = await _handler(invocation, cancellationToken).ConfigureAwait(false);
        _commands.Enqueue(new RecordedCommand(invocation, result));
        return result;
    }
}
