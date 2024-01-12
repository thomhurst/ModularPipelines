using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudApiGateway
{
    public GcloudApiGateway(
        GcloudApiGatewayApiConfigs apiConfigs,
        GcloudApiGatewayApis apis,
        GcloudApiGatewayGateways gateways,
        GcloudApiGatewayOperations operations
    )
    {
        ApiConfigs = apiConfigs;
        Apis = apis;
        Gateways = gateways;
        Operations = operations;
    }

    public GcloudApiGatewayApiConfigs ApiConfigs { get; }

    public GcloudApiGatewayApis Apis { get; }

    public GcloudApiGatewayGateways Gateways { get; }

    public GcloudApiGatewayOperations Operations { get; }
}