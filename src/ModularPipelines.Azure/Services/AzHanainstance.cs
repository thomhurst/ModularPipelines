using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzHanainstanceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzHanainstanceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceListOptions(), token);
    }

    public async Task<CommandResult> Restart(AzHanainstanceRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzHanainstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceShowOptions(), token);
    }

    public async Task<CommandResult> Shutdown(AzHanainstanceShutdownOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceShutdownOptions(), token);
    }

    public async Task<CommandResult> Start(AzHanainstanceStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceStartOptions(), token);
    }

    public async Task<CommandResult> Update(AzHanainstanceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHanainstanceUpdateOptions(), token);
    }
}

