using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudFilestore
{
    public GcloudFilestore(
        GcloudFilestoreBackups backups,
        GcloudFilestoreInstances instances,
        GcloudFilestoreLocations locations,
        GcloudFilestoreOperations operations,
        GcloudFilestoreRegions regions,
        GcloudFilestoreZones zones
    )
    {
        Backups = backups;
        Instances = instances;
        Locations = locations;
        Operations = operations;
        Regions = regions;
        Zones = zones;
    }

    public GcloudFilestoreBackups Backups { get; }

    public GcloudFilestoreInstances Instances { get; }

    public GcloudFilestoreLocations Locations { get; }

    public GcloudFilestoreOperations Operations { get; }

    public GcloudFilestoreRegions Regions { get; }

    public GcloudFilestoreZones Zones { get; }
}