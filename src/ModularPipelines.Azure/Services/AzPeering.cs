using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzPeering
{
    public AzPeering(
        AzPeeringAsn asn,
        AzPeeringLegacy legacy,
        AzPeeringLocation location,
        AzPeeringPeering peering,
        AzPeeringReceivedRoute receivedRoute,
        AzPeeringRegisteredAsn registeredAsn,
        AzPeeringRegisteredPrefix registeredPrefix,
        AzPeeringService service
    )
    {
        Asn = asn;
        Legacy = legacy;
        Location = location;
        Peering = peering;
        ReceivedRoute = receivedRoute;
        RegisteredAsn = registeredAsn;
        RegisteredPrefix = registeredPrefix;
        Service = service;
    }

    public AzPeeringAsn Asn { get; }

    public AzPeeringLegacy Legacy { get; }

    public AzPeeringLocation Location { get; }

    public AzPeeringPeering Peering { get; }

    public AzPeeringReceivedRoute ReceivedRoute { get; }

    public AzPeeringRegisteredAsn RegisteredAsn { get; }

    public AzPeeringRegisteredPrefix RegisteredPrefix { get; }

    public AzPeeringService Service { get; }
}