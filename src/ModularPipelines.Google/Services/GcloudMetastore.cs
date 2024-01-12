using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudMetastore
{
    public GcloudMetastore(
        GcloudMetastoreFederations federations,
        GcloudMetastoreLocations locations,
        GcloudMetastoreOperations operations,
        GcloudMetastoreServices services
    )
    {
        Federations = federations;
        Locations = locations;
        Operations = operations;
        Services = services;
    }

    public GcloudMetastoreFederations Federations { get; }

    public GcloudMetastoreLocations Locations { get; }

    public GcloudMetastoreOperations Operations { get; }

    public GcloudMetastoreServices Services { get; }
}