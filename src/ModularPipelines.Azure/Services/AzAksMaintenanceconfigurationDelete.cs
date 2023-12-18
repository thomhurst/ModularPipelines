using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "maintenanceconfiguration")]
public class AzAksMaintenanceconfigurationDelete
{
    public AzAksMaintenanceconfigurationDelete(
        AzAksMaintenanceconfigurationDeleteAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksMaintenanceconfigurationDeleteAksPreview AksPreview { get; }
}

