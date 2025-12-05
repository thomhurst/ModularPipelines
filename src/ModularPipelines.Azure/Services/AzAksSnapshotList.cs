using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "snapshot")]
public class AzAksSnapshotList
{
    public AzAksSnapshotList(
        AzAksSnapshotListAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksSnapshotListAksPreview AksPreview { get; }
}