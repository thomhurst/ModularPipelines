using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool")]
public class AzAksNodepoolStart
{
    public AzAksNodepoolStart(
        AzAksNodepoolStartAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolStartAksPreview AksPreview { get; }
}