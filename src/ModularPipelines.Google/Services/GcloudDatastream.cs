using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudDatastream
{
    public GcloudDatastream(
        GcloudDatastreamConnectionProfiles connectionProfiles,
        GcloudDatastreamLocations locations,
        GcloudDatastreamObjects objects,
        GcloudDatastreamOperations operations,
        GcloudDatastreamPrivateConnections privateConnections,
        GcloudDatastreamRoutes routes,
        GcloudDatastreamStreams streams
    )
    {
        ConnectionProfiles = connectionProfiles;
        Locations = locations;
        Objects = objects;
        Operations = operations;
        PrivateConnections = privateConnections;
        Routes = routes;
        Streams = streams;
    }

    public GcloudDatastreamConnectionProfiles ConnectionProfiles { get; }

    public GcloudDatastreamLocations Locations { get; }

    public GcloudDatastreamObjects Objects { get; }

    public GcloudDatastreamOperations Operations { get; }

    public GcloudDatastreamPrivateConnections PrivateConnections { get; }

    public GcloudDatastreamRoutes Routes { get; }

    public GcloudDatastreamStreams Streams { get; }
}