using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "maintenanceconfiguration")]
public class AzAksMaintenanceconfigurationList
{
    public AzAksMaintenanceconfigurationList(
        AzAksMaintenanceconfigurationListAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksMaintenanceconfigurationListAksPreview AksPreview { get; }
}