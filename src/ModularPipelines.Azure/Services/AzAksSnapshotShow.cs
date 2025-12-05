using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "snapshot")]
public class AzAksSnapshotShow
{
    public AzAksSnapshotShow(
        AzAksSnapshotShowAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksSnapshotShowAksPreview AksPreview { get; }
}