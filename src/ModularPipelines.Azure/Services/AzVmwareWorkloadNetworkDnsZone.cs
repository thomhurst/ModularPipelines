using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "workload-network")]
public class AzVmwareWorkloadNetworkDnsZone
{
    public AzVmwareWorkloadNetworkDnsZone(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzVmwareWorkloadNetworkDnsZoneCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzVmwareWorkloadNetworkDnsZoneDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzVmwareWorkloadNetworkDnsZoneListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzVmwareWorkloadNetworkDnsZoneShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkDnsZoneShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzVmwareWorkloadNetworkDnsZoneUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkDnsZoneUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzVmwareWorkloadNetworkDnsZoneWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkDnsZoneWaitOptions(), token);
    }
}