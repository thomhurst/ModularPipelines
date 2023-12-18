using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads")]
public class AzWorkloadsSapDatabaseInstance
{
    public AzWorkloadsSapDatabaseInstance(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzWorkloadsSapDatabaseInstanceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzWorkloadsSapDatabaseInstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapDatabaseInstanceShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzWorkloadsSapDatabaseInstanceStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapDatabaseInstanceStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzWorkloadsSapDatabaseInstanceStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapDatabaseInstanceStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzWorkloadsSapDatabaseInstanceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapDatabaseInstanceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzWorkloadsSapDatabaseInstanceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapDatabaseInstanceWaitOptions(), token);
    }
}