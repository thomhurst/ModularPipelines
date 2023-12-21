using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "snapshot")]
public class AzAksSnapshotCreate
{
    public AzAksSnapshotCreate(
        AzAksSnapshotCreateAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksSnapshotCreateAksPreview AksPreview { get; }
}