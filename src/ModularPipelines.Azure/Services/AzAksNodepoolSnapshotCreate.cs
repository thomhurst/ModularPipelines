using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "nodepool", "snapshot")]
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