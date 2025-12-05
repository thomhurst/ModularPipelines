using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "nodepool")]
public class AzAksNodepoolScale
{
    public AzAksNodepoolScale(
        AzAksNodepoolScaleAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolScaleAksPreview AksPreview { get; }
}