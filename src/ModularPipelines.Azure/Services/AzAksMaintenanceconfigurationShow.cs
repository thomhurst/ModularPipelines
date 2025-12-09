using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "maintenanceconfiguration")]
public class AzAksMaintenanceconfigurationShow
{
    public AzAksMaintenanceconfigurationShow(
        AzAksMaintenanceconfigurationShowAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksMaintenanceconfigurationShowAksPreview AksPreview { get; }
}