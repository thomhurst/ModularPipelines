using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks")]
public class AzAksEnableAddons
{
    public AzAksEnableAddons(
        AzAksEnableAddonsAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksEnableAddonsAksPreview AksPreview { get; }
}