using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "maintenanceconfiguration")]
public class AzAksMaintenanceconfigurationAdd
{
    public AzAksMaintenanceconfigurationAdd(
        AzAksMaintenanceconfigurationAddAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksMaintenanceconfigurationAddAksPreview AksPreview { get; }
}

