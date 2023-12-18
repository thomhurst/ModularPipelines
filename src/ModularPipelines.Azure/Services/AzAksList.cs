using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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