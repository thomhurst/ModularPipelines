using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudNetworkConnectivity
{
    public GcloudNetworkConnectivity(
        GcloudNetworkConnectivityHubs hubs,
        GcloudNetworkConnectivityInternalRanges internalRanges,
        GcloudNetworkConnectivityLocations locations,
        GcloudNetworkConnectivityOperations operations,
        GcloudNetworkConnectivityPolicyBasedRoutes policyBasedRoutes,
        GcloudNetworkConnectivityServiceConnectionPolicies serviceConnectionPolicies,
        GcloudNetworkConnectivitySpokes spokes
    )
    {
        Hubs = hubs;
        InternalRanges = internalRanges;
        Locations = locations;
        Operations = operations;
        PolicyBasedRoutes = policyBasedRoutes;
        ServiceConnectionPolicies = serviceConnectionPolicies;
        Spokes = spokes;
    }

    public GcloudNetworkConnectivityHubs Hubs { get; }

    public GcloudNetworkConnectivityInternalRanges InternalRanges { get; }

    public GcloudNetworkConnectivityLocations Locations { get; }

    public GcloudNetworkConnectivityOperations Operations { get; }

    public GcloudNetworkConnectivityPolicyBasedRoutes PolicyBasedRoutes { get; }

    public GcloudNetworkConnectivityServiceConnectionPolicies ServiceConnectionPolicies { get; }

    public GcloudNetworkConnectivitySpokes Spokes { get; }
}