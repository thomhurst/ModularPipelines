using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

