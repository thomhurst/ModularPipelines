using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzDesktopvirtualization
{
    public AzDesktopvirtualization(
        AzDesktopvirtualizationApplicationgroup applicationgroup,
        AzDesktopvirtualizationHostpool hostpool,
        AzDesktopvirtualizationWorkspace workspace
    )
    {
        Applicationgroup = applicationgroup;
        Hostpool = hostpool;
        Workspace = workspace;
    }

    public AzDesktopvirtualizationApplicationgroup Applicationgroup { get; }

    public AzDesktopvirtualizationHostpool Hostpool { get; }

    public AzDesktopvirtualizationWorkspace Workspace { get; }
}