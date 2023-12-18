using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkVpnServerConfig
{
    public AzNetworkVpnServerConfig(
        AzNetworkVpnServerConfigIpsecPolicy ipsecPolicy,
        ICommand internalCommand
    )
    {
        IpsecPolicy = ipsecPolicy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkVpnServerConfigIpsecPolicy IpsecPolicy { get; }

    public async Task<CommandResult> Create(AzNetworkVpnServerConfigCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkVpnServerConfigDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkVpnServerConfigListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnServerConfigListOptions(), token);
    }

    public async Task<CommandResult> Set(AzNetworkVpnServerConfigSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkVpnServerConfigShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzNetworkVpnServerConfigWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}