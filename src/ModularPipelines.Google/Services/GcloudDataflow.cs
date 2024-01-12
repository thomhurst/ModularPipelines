using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudDataflow
{
    public GcloudDataflow(
        GcloudDataflowFlexTemplate flexTemplate,
        GcloudDataflowJobs jobs,
        GcloudDataflowSnapshots snapshots,
        GcloudDataflowSql sql
    )
    {
        FlexTemplate = flexTemplate;
        Jobs = jobs;
        Snapshots = snapshots;
        Sql = sql;
    }

    public GcloudDataflowFlexTemplate FlexTemplate { get; }

    public GcloudDataflowJobs Jobs { get; }

    public GcloudDataflowSnapshots Snapshots { get; }

    public GcloudDataflowSql Sql { get; }
}