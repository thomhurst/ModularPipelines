using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("workloads")]
public class AzWorkloadsSapVirtualInstance
{
    public AzWorkloadsSapVirtualInstance(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzWorkloadsSapVirtualInstanceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzWorkloadsSapVirtualInstanceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzWorkloadsSapVirtualInstanceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzWorkloadsSapVirtualInstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzWorkloadsSapVirtualInstanceStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzWorkloadsSapVirtualInstanceStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzWorkloadsSapVirtualInstanceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzWorkloadsSapVirtualInstanceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceWaitOptions(), cancellationToken: token);
    }
}