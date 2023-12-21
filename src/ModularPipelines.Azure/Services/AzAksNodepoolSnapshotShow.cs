using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool", "snapshot")]
public class AzAksNodepoolSnapshotShow
{
    public AzAksNodepoolSnapshotShow(
        AzAksNodepoolSnapshotShowAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolSnapshotShowAksPreview AksPreview { get; }
}