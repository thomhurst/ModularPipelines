using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
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