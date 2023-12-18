using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzLargeInstance
{
    public AzLargeInstance(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzLargeInstanceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLargeInstanceListOptions(), token);
    }

    public async Task<CommandResult> Restart(AzLargeInstanceRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLargeInstanceRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzLargeInstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLargeInstanceShowOptions(), token);
    }

    public async Task<CommandResult> Shutdown(AzLargeInstanceShutdownOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLargeInstanceShutdownOptions(), token);
    }

    public async Task<CommandResult> Start(AzLargeInstanceStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLargeInstanceStartOptions(), token);
    }

    public async Task<CommandResult> Update(AzLargeInstanceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLargeInstanceUpdateOptions(), token);
    }
}