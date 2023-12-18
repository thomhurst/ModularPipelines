using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDnsResolver
{
    public AzDnsResolver(
        AzDnsResolverForwardingRule forwardingRule,
        AzDnsResolverForwardingRuleset forwardingRuleset,
        AzDnsResolverInboundEndpoint inboundEndpoint,
        AzDnsResolverOutboundEndpoint outboundEndpoint,
        AzDnsResolverVnetLink vnetLink,
        ICommand internalCommand
    )
    {
        ForwardingRule = forwardingRule;
        ForwardingRuleset = forwardingRuleset;
        InboundEndpoint = inboundEndpoint;
        OutboundEndpoint = outboundEndpoint;
        VnetLink = vnetLink;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDnsResolverForwardingRule ForwardingRule { get; }

    public AzDnsResolverForwardingRuleset ForwardingRuleset { get; }

    public AzDnsResolverInboundEndpoint InboundEndpoint { get; }

    public AzDnsResolverOutboundEndpoint OutboundEndpoint { get; }

    public AzDnsResolverVnetLink VnetLink { get; }

    public async Task<CommandResult> Create(AzDnsResolverCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDnsResolverDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDnsResolverListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListByVirtualNetwork(AzDnsResolverListByVirtualNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDnsResolverShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDnsResolverUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDnsResolverWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverWaitOptions(), token);
    }
}