using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("network")]
public class AzNetworkPrivateEndpoint
{
    public AzNetworkPrivateEndpoint(
        AzNetworkPrivateEndpointAsg asg,
        AzNetworkPrivateEndpointDnsZoneGroup dnsZoneGroup,
        AzNetworkPrivateEndpointIpConfig ipConfig,
        ICommand internalCommand
    )
    {
        Asg = asg;
        DnsZoneGroup = dnsZoneGroup;
        IpConfig = ipConfig;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkPrivateEndpointAsg Asg { get; }

    public AzNetworkPrivateEndpointDnsZoneGroup DnsZoneGroup { get; }

    public AzNetworkPrivateEndpointIpConfig IpConfig { get; }

    public async Task<CommandResult> Create(AzNetworkPrivateEndpointCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkPrivateEndpointDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkPrivateEndpointListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListTypes(AzNetworkPrivateEndpointListTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointListTypesOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkPrivateEndpointShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkPrivateEndpointUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkPrivateEndpointWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointWaitOptions(), cancellationToken: token);
    }
}