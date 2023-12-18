using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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