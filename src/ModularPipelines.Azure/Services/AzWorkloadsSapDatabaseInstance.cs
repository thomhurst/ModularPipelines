using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("workloads")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzWorkloadsSapDatabaseInstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapDatabaseInstanceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzWorkloadsSapDatabaseInstanceStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapDatabaseInstanceStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzWorkloadsSapDatabaseInstanceStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapDatabaseInstanceStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzWorkloadsSapDatabaseInstanceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapDatabaseInstanceUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzWorkloadsSapDatabaseInstanceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapDatabaseInstanceWaitOptions(), cancellationToken: token);
    }
}