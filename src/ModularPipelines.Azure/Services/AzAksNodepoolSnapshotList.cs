using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "nodepool", "snapshot")]
public class AzAksNodepoolSnapshotList
{
    public AzAksNodepoolSnapshotList(
        AzAksNodepoolSnapshotListAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolSnapshotListAksPreview AksPreview { get; }
}