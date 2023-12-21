using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksDelete
{
    public AzAksDelete(
        AzAksDeleteAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksDeleteAksPreview AksPreview { get; }
}