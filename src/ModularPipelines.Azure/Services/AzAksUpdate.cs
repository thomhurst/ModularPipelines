using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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