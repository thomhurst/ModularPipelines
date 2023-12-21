using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool")]
public class AzAksNodepoolDelete
{
    public AzAksNodepoolDelete(
        AzAksNodepoolDeleteAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolDeleteAksPreview AksPreview { get; }
}