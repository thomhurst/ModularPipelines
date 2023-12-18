using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksStop
{
    public AzAksStop(
        AzAksStopAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksStopAksPreview AksPreview { get; }
}

