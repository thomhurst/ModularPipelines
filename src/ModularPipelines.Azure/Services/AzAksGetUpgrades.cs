using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks")]
public class AzAksGetUpgrades
{
    public AzAksGetUpgrades(
        AzAksGetUpgradesAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksGetUpgradesAksPreview AksPreview { get; }
}