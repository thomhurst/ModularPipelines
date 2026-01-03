using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("workloads")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzWorkloadsSapApplicationServerInstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapApplicationServerInstanceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzWorkloadsSapApplicationServerInstanceStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapApplicationServerInstanceStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzWorkloadsSapApplicationServerInstanceStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapApplicationServerInstanceStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzWorkloadsSapApplicationServerInstanceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapApplicationServerInstanceUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzWorkloadsSapApplicationServerInstanceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapApplicationServerInstanceWaitOptions(), cancellationToken: token);
    }
}