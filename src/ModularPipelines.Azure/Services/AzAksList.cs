using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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