using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
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