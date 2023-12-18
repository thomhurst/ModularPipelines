using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzCdn
{
    public AzCdn(
        AzCdnCustomDomain customDomain,
        AzCdnEdgeNode edgeNode,
        AzCdnEndpoint endpoint,
        AzCdnOrigin origin,
        AzCdnOriginGroup originGroup,
        AzCdnProfile profile,
        ICommand internalCommand
    )
    {
        CustomDomain = customDomain;
        EdgeNode = edgeNode;
        Endpoint = endpoint;
        Origin = origin;
        OriginGroup = originGroup;
        Profile = profile;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCdnCustomDomain CustomDomain { get; }

    public AzCdnEdgeNode EdgeNode { get; }

    public AzCdnEndpoint Endpoint { get; }

    public AzCdnOrigin Origin { get; }

    public AzCdnOriginGroup OriginGroup { get; }

    public AzCdnProfile Profile { get; }

    public async Task<CommandResult> NameExists(AzCdnNameExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnNameExistsOptions(), token);
    }

    public async Task<CommandResult> Usage(AzCdnUsageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnUsageOptions(), token);
    }
}