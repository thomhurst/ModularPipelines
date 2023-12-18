using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "workload-network")]
public class AzVmwareWorkloadNetworkDhcp
{
    public AzVmwareWorkloadNetworkDhcp(
        AzVmwareWorkloadNetworkDhcpRelay relay,
        AzVmwareWorkloadNetworkDhcpServer server,
        ICommand internalCommand
    )
    {
        Relay = relay;
        Server = server;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzVmwareWorkloadNetworkDhcpRelay Relay { get; }

    public AzVmwareWorkloadNetworkDhcpServer Server { get; }

    public async Task<CommandResult> List(AzVmwareWorkloadNetworkDhcpListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzVmwareWorkloadNetworkDhcpShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwareWorkloadNetworkDhcpShowOptions(), token);
    }
}