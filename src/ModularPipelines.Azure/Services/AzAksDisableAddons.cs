using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

