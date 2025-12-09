using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("dns-resolver")]
public class AzDnsResolverOutboundEndpoint
{
    public AzDnsResolverOutboundEndpoint(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDnsResolverOutboundEndpointCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDnsResolverOutboundEndpointDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverOutboundEndpointDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDnsResolverOutboundEndpointListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDnsResolverOutboundEndpointShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverOutboundEndpointShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDnsResolverOutboundEndpointUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverOutboundEndpointUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDnsResolverOutboundEndpointWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDnsResolverOutboundEndpointWaitOptions(), token);
    }
}