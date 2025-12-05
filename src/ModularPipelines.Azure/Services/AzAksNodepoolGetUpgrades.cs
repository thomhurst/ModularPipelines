using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "nodepool")]
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