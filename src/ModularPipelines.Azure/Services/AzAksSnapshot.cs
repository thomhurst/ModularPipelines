using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksSnapshot
{
    public AzAksSnapshot(
        AzAksSnapshotCreate create,
        AzAksSnapshotDelete delete,
        AzAksSnapshotList list,
        AzAksSnapshotShow show
    )
    {
        Create = create;
        Delete = delete;
        List = list;
        Show = show;
    }

    public AzAksSnapshotCreate Create { get; }

    public AzAksSnapshotDelete Delete { get; }

    public AzAksSnapshotList List { get; }

    public AzAksSnapshotShow Show { get; }
}