using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksCreate
{
    public AzAksCreate(
        AzAksCreateAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksCreateAksPreview AksPreview { get; }
}