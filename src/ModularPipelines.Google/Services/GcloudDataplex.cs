using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudDataplex
{
    public GcloudDataplex(
        GcloudDataplexAssets assets,
        GcloudDataplexContent content,
        GcloudDataplexDatascans datascans,
        GcloudDataplexEnvironments environments,
        GcloudDataplexLakes lakes,
        GcloudDataplexTasks tasks,
        GcloudDataplexZones zones
    )
    {
        Assets = assets;
        Content = content;
        Datascans = datascans;
        Environments = environments;
        Lakes = lakes;
        Tasks = tasks;
        Zones = zones;
    }

    public GcloudDataplexAssets Assets { get; }

    public GcloudDataplexContent Content { get; }

    public GcloudDataplexDatascans Datascans { get; }

    public GcloudDataplexEnvironments Environments { get; }

    public GcloudDataplexLakes Lakes { get; }

    public GcloudDataplexTasks Tasks { get; }

    public GcloudDataplexZones Zones { get; }
}