using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

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