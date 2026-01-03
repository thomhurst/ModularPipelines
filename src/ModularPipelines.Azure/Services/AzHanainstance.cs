using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzHanainstance
{
    public AzHanainstance(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzHanainstanceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzHanainstanceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzHanainstanceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Restart(AzHanainstanceRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceRestartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzHanainstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Shutdown(AzHanainstanceShutdownOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceShutdownOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzHanainstanceStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzHanainstanceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceUpdateOptions(), cancellationToken: token);
    }
}