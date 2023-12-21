using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksBrowse
{
    public AzAksBrowse(
        AzAksBrowseAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksBrowseAksPreview AksPreview { get; }
}