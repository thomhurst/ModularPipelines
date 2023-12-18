using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "workload-network")]
public class AzVmwareWorkloadNetworkPublicIp
{
    public AzVmwareWorkloadNetworkPublicIp(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzVmwareWorkloadNetworkPublicIpCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzVmwareWorkloadNetworkPublicIpDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkPublicIpDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzVmwareWorkloadNetworkPublicIpListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzVmwareWorkloadNetworkPublicIpShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkPublicIpShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzVmwareWorkloadNetworkPublicIpWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkPublicIpWaitOptions(), token);
    }
}