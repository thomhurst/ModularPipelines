using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks")]
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