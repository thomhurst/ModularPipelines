using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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