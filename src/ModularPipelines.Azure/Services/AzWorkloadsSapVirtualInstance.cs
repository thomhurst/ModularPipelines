using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzWorkloadsSapVirtualInstanceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzWorkloadsSapVirtualInstanceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceListOptions(), token);
    }

    public async Task<CommandResult> Show(AzWorkloadsSapVirtualInstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzWorkloadsSapVirtualInstanceStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzWorkloadsSapVirtualInstanceStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzWorkloadsSapVirtualInstanceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzWorkloadsSapVirtualInstanceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapVirtualInstanceWaitOptions(), token);
    }
}