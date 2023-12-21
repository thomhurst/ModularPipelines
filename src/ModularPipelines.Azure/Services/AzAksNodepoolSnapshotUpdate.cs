using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool", "snapshot")]
public class AzAksNodepoolSnapshotUpdate
{
    public AzAksNodepoolSnapshotUpdate(
        AzAksNodepoolSnapshotUpdateAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolSnapshotUpdateAksPreview AksPreview { get; }
}