using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "workload-network")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzVmwareWorkloadNetworkDnsZoneDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkDnsZoneDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzVmwareWorkloadNetworkDnsZoneListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzVmwareWorkloadNetworkDnsZoneShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkDnsZoneShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzVmwareWorkloadNetworkDnsZoneUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkDnsZoneUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzVmwareWorkloadNetworkDnsZoneWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkDnsZoneWaitOptions(), cancellationToken: token);
    }
}