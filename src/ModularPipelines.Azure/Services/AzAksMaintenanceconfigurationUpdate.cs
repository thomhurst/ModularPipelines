using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "maintenanceconfiguration")]
public class AzAksMaintenanceconfigurationUpdate
{
    public AzAksMaintenanceconfigurationUpdate(
        AzAksMaintenanceconfigurationUpdateAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksMaintenanceconfigurationUpdateAksPreview AksPreview { get; }
}