using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "maintenanceconfiguration")]
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