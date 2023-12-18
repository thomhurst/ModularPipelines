using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud")]
public class AzSpringCloudGateway
{
    public AzSpringCloudGateway(
        AzSpringCloudGatewayCustomDomain customDomain,
        AzSpringCloudGatewayRouteConfig routeConfig,
        ICommand internalCommand
    )
    {
        CustomDomain = customDomain;
        RouteConfig = routeConfig;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringCloudGatewayCustomDomain CustomDomain { get; }

    public AzSpringCloudGatewayRouteConfig RouteConfig { get; }

    public async Task<CommandResult> Clear(AzSpringCloudGatewayClearOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringCloudGatewayShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSpringCloudGatewayUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}