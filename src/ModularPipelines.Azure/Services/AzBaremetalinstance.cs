using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzBaremetalinstance
{
    public AzBaremetalinstance(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Delete(AzBaremetalinstanceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBaremetalinstanceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzBaremetalinstanceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBaremetalinstanceListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Restart(AzBaremetalinstanceRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBaremetalinstanceRestartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzBaremetalinstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBaremetalinstanceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Shutdown(AzBaremetalinstanceShutdownOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBaremetalinstanceShutdownOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzBaremetalinstanceStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBaremetalinstanceStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzBaremetalinstanceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBaremetalinstanceUpdateOptions(), cancellationToken: token);
    }
}