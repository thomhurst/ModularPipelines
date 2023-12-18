using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksScale
{
    public AzAksScale(
        AzAksScaleAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksScaleAksPreview AksPreview { get; }
}