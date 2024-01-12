using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudNetworkServices
{
    public GcloudNetworkServices(
        GcloudNetworkServicesEndpointPolicies endpointPolicies,
        GcloudNetworkServicesGateways gateways,
        GcloudNetworkServicesGrpcRoutes grpcRoutes,
        GcloudNetworkServicesHttpRoutes httpRoutes,
        GcloudNetworkServicesMeshes meshes,
        GcloudNetworkServicesServiceBindings serviceBindings,
        GcloudNetworkServicesTcpRoutes tcpRoutes,
        GcloudNetworkServicesTlsRoutes tlsRoutes
    )
    {
        EndpointPolicies = endpointPolicies;
        Gateways = gateways;
        GrpcRoutes = grpcRoutes;
        HttpRoutes = httpRoutes;
        Meshes = meshes;
        ServiceBindings = serviceBindings;
        TcpRoutes = tcpRoutes;
        TlsRoutes = tlsRoutes;
    }

    public GcloudNetworkServicesEndpointPolicies EndpointPolicies { get; }

    public GcloudNetworkServicesGateways Gateways { get; }

    public GcloudNetworkServicesGrpcRoutes GrpcRoutes { get; }

    public GcloudNetworkServicesHttpRoutes HttpRoutes { get; }

    public GcloudNetworkServicesMeshes Meshes { get; }

    public GcloudNetworkServicesServiceBindings ServiceBindings { get; }

    public GcloudNetworkServicesTcpRoutes TcpRoutes { get; }

    public GcloudNetworkServicesTlsRoutes TlsRoutes { get; }
}