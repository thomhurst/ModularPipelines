using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool")]
public class AzAksNodepoolGetUpgrades
{
    public AzAksNodepoolGetUpgrades(
        AzAksNodepoolGetUpgradesAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolGetUpgradesAksPreview AksPreview { get; }
}