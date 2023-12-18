using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
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

    public async Task<CommandResult> Delete(AzNetworkVpnConnectionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkVpnConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListIkeSas(AzNetworkVpnConnectionListIkeSasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkVpnConnectionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
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