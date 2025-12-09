using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway")]
public class AzNetworkApplicationGatewayPrivateLink
{
    public AzNetworkApplicationGatewayPrivateLink(
        AzNetworkApplicationGatewayPrivateLinkIpConfig ipConfig,
        ICommand internalCommand
    )
    {
        IpConfig = ipConfig;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkApplicationGatewayPrivateLinkIpConfig IpConfig { get; }

    public async Task<CommandResult> Add(AzNetworkApplicationGatewayPrivateLinkAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkApplicationGatewayPrivateLinkListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzNetworkApplicationGatewayPrivateLinkRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkApplicationGatewayPrivateLinkShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzNetworkApplicationGatewayPrivateLinkWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkApplicationGatewayPrivateLinkWaitOptions(), token);
    }
}