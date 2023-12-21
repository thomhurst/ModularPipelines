using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool")]
public class AzAksNodepoolAdd
{
    public AzAksNodepoolAdd(
        AzAksNodepoolAddAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolAddAksPreview AksPreview { get; }
}