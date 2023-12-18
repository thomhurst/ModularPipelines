using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads")]
public class AzWorkloadsSapApplicationServerInstance
{
    public AzWorkloadsSapApplicationServerInstance(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzWorkloadsSapApplicationServerInstanceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzWorkloadsSapApplicationServerInstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapApplicationServerInstanceShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzWorkloadsSapApplicationServerInstanceStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapApplicationServerInstanceStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzWorkloadsSapApplicationServerInstanceStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapApplicationServerInstanceStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzWorkloadsSapApplicationServerInstanceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapApplicationServerInstanceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzWorkloadsSapApplicationServerInstanceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapApplicationServerInstanceWaitOptions(), token);
    }
}