using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudBigtable
{
    public GcloudBigtable(
        GcloudBigtableAppProfiles appProfiles,
        GcloudBigtableBackups backups,
        GcloudBigtableClusters clusters,
        GcloudBigtableHotTablets hotTablets,
        GcloudBigtableInstances instances,
        GcloudBigtableOperations operations
    )
    {
        AppProfiles = appProfiles;
        Backups = backups;
        Clusters = clusters;
        HotTablets = hotTablets;
        Instances = instances;
        Operations = operations;
    }

    public GcloudBigtableAppProfiles AppProfiles { get; }

    public GcloudBigtableBackups Backups { get; }

    public GcloudBigtableClusters Clusters { get; }

    public GcloudBigtableHotTablets HotTablets { get; }

    public GcloudBigtableInstances Instances { get; }

    public GcloudBigtableOperations Operations { get; }
}