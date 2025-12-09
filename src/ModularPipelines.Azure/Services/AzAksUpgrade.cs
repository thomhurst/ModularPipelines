using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks")]
public class AzAksUpgrade
{
    public AzAksUpgrade(
        AzAksUpgradeAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksUpgradeAksPreview AksPreview { get; }
}