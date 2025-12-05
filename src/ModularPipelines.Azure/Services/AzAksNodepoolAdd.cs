using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "nodepool")]
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