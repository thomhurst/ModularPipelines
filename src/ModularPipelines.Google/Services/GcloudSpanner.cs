using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudSpanner
{
    public GcloudSpanner(
        GcloudSpannerBackups backups,
        GcloudSpannerDatabases databases,
        GcloudSpannerInstanceConfigs instanceConfigs,
        GcloudSpannerInstances instances,
        GcloudSpannerOperations operations,
        GcloudSpannerRows rows,
        GcloudSpannerSamples samples
    )
    {
        Backups = backups;
        Databases = databases;
        InstanceConfigs = instanceConfigs;
        Instances = instances;
        Operations = operations;
        Rows = rows;
        Samples = samples;
    }

    public GcloudSpannerBackups Backups { get; }

    public GcloudSpannerDatabases Databases { get; }

    public GcloudSpannerInstanceConfigs InstanceConfigs { get; }

    public GcloudSpannerInstances Instances { get; }

    public GcloudSpannerOperations Operations { get; }

    public GcloudSpannerRows Rows { get; }

    public GcloudSpannerSamples Samples { get; }
}