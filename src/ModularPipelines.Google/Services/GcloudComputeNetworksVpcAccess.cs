using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks")]
public class GcloudComputeNetworksVpcAccess
{
    public GcloudComputeNetworksVpcAccess(
        GcloudComputeNetworksVpcAccessConnectors connectors,
        GcloudComputeNetworksVpcAccessLocations locations,
        GcloudComputeNetworksVpcAccessOperations operations
    )
    {
        Connectors = connectors;
        Locations = locations;
        Operations = operations;
    }

    public GcloudComputeNetworksVpcAccessConnectors Connectors { get; }

    public GcloudComputeNetworksVpcAccessLocations Locations { get; }

    public GcloudComputeNetworksVpcAccessOperations Operations { get; }
}