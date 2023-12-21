using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksList
{
    public AzAksList(
        AzAksListAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksListAksPreview AksPreview { get; }
}