using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksWait
{
    public AzAksWait(
        AzAksWaitAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksWaitAksPreview AksPreview { get; }
}