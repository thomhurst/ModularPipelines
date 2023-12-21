using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool", "snapshot")]
public class AzAksNodepoolSnapshotCreate
{
    public AzAksNodepoolSnapshotCreate(
        AzAksNodepoolSnapshotCreateAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolSnapshotCreateAksPreview AksPreview { get; }
}