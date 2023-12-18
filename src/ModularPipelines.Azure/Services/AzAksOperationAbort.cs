using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksOperationAbort
{
    public AzAksOperationAbort(
        AzAksOperationAbortAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksOperationAbortAksPreview AksPreview { get; }
}