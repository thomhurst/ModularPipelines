using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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