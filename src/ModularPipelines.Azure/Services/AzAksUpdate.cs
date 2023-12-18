using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksUpdate
{
    public AzAksUpdate(
        AzAksUpdateAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksUpdateAksPreview AksPreview { get; }
}

