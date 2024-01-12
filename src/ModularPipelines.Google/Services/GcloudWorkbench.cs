using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudWorkbench
{
    public GcloudWorkbench(
        GcloudWorkbenchInstances instances
    )
    {
        Instances = instances;
    }

    public GcloudWorkbenchInstances Instances { get; }
}