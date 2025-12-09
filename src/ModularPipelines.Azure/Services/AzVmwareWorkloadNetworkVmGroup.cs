using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "workload-network")]
public class AzVmwareWorkloadNetworkVmGroup
{
    public AzVmwareWorkloadNetworkVmGroup(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzVmwareWorkloadNetworkVmGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzVmwareWorkloadNetworkVmGroupDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkVmGroupDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzVmwareWorkloadNetworkVmGroupListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzVmwareWorkloadNetworkVmGroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkVmGroupShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzVmwareWorkloadNetworkVmGroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkVmGroupUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzVmwareWorkloadNetworkVmGroupWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkVmGroupWaitOptions(), token);
    }
}