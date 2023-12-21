using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksDisableAddons
{
    public AzAksDisableAddons(
        AzAksDisableAddonsAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksDisableAddonsAksPreview AksPreview { get; }
}