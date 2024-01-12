using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudScheduler
{
    public GcloudScheduler(
        GcloudSchedulerJobs jobs,
        GcloudSchedulerLocations locations
    )
    {
        Jobs = jobs;
        Locations = locations;
    }

    public GcloudSchedulerJobs Jobs { get; }

    public GcloudSchedulerLocations Locations { get; }
}