using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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