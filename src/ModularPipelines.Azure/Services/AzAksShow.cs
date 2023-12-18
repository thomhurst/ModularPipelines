using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksShow
{
    public AzAksShow(
        AzAksShowAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksShowAksPreview AksPreview { get; }
}