using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudNotebooks
{
    public GcloudNotebooks(
        GcloudNotebooksEnvironments environments,
        GcloudNotebooksInstances instances,
        GcloudNotebooksLocations locations,
        GcloudNotebooksRuntimes runtimes
    )
    {
        Environments = environments;
        Instances = instances;
        Locations = locations;
        Runtimes = runtimes;
    }

    public GcloudNotebooksEnvironments Environments { get; }

    public GcloudNotebooksInstances Instances { get; }

    public GcloudNotebooksLocations Locations { get; }

    public GcloudNotebooksRuntimes Runtimes { get; }
}