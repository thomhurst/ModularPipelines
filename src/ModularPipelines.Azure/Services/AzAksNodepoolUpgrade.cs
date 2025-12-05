using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "nodepool")]
public class AzAksNodepoolUpgrade
{
    public AzAksNodepoolUpgrade(
        AzAksNodepoolUpgradeAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolUpgradeAksPreview AksPreview { get; }
}