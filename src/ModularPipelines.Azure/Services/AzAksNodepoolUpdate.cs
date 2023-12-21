using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool")]
public class AzAksNodepoolUpdate
{
    public AzAksNodepoolUpdate(
        AzAksNodepoolUpdateAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolUpdateAksPreview AksPreview { get; }
}