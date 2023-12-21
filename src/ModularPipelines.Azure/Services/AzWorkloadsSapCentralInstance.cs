using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads")]
public class AzWorkloadsSapCentralInstance
{
    public AzWorkloadsSapCentralInstance(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzWorkloadsSapCentralInstanceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzWorkloadsSapCentralInstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapCentralInstanceShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzWorkloadsSapCentralInstanceStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapCentralInstanceStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzWorkloadsSapCentralInstanceStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapCentralInstanceStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzWorkloadsSapCentralInstanceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapCentralInstanceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzWorkloadsSapCentralInstanceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapCentralInstanceWaitOptions(), token);
    }
}