using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudAssured
{
    public GcloudAssured(
        GcloudAssuredOperations operations,
        GcloudAssuredWorkloads workloads
    )
    {
        Operations = operations;
        Workloads = workloads;
    }

    public GcloudAssuredOperations Operations { get; }

    public GcloudAssuredWorkloads Workloads { get; }
}