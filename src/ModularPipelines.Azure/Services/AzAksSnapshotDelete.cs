using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "snapshot")]
public class AzAksSnapshotDelete
{
    public AzAksSnapshotDelete(
        AzAksSnapshotDeleteAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksSnapshotDeleteAksPreview AksPreview { get; }
}