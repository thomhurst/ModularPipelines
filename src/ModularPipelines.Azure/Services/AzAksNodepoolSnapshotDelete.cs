using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool", "snapshot")]
public class AzAksNodepoolSnapshotDelete
{
    public AzAksNodepoolSnapshotDelete(
        AzAksNodepoolSnapshotDeleteAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolSnapshotDeleteAksPreview AksPreview { get; }
}