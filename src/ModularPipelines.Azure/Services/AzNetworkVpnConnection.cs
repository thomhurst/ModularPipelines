using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network")]
public class AzNetworkVpnConnection
{
    public AzNetworkVpnConnection(
        AzNetworkVpnConnectionIpsecPolicy ipsecPolicy,
        AzNetworkVpnConnectionPacketCapture packetCapture,
        AzNetworkVpnConnectionSharedKey sharedKey,
        ICommand internalCommand
    )
    {
        IpsecPolicy = ipsecPolicy;
        PacketCapture = packetCapture;
        SharedKey = sharedKey;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkVpnConnectionIpsecPolicy IpsecPolicy { get; }

    public AzNetworkVpnConnectionPacketCapture PacketCapture { get; }

    public AzNetworkVpnConnectionSharedKey SharedKey { get; }

    public async Task<CommandResult> Create(AzNetworkVpnConnectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkVpnConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkVpnConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListIkeSas(AzNetworkVpnConnectionListIkeSasOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnConnectionListIkeSasOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkVpnConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnConnectionShowOptions(), token);
    }

    public async Task<CommandResult> ShowDeviceConfigScript(AzNetworkVpnConnectionShowDeviceConfigScriptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzNetworkVpnConnectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnConnectionUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkVpnConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnConnectionWaitOptions(), token);
    }
}