using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool")]
public class AzAksNodepoolList
{
    public AzAksNodepoolList(
        AzAksNodepoolListAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolListAksPreview AksPreview { get; }
}