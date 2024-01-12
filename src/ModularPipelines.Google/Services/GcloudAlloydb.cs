using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudAlloydb
{
    public GcloudAlloydb(
        GcloudAlloydbBackups backups,
        GcloudAlloydbClusters clusters,
        GcloudAlloydbInstances instances,
        GcloudAlloydbOperations operations,
        GcloudAlloydbUsers users
    )
    {
        Backups = backups;
        Clusters = clusters;
        Instances = instances;
        Operations = operations;
        Users = users;
    }

    public GcloudAlloydbBackups Backups { get; }

    public GcloudAlloydbClusters Clusters { get; }

    public GcloudAlloydbInstances Instances { get; }

    public GcloudAlloydbOperations Operations { get; }

    public GcloudAlloydbUsers Users { get; }
}