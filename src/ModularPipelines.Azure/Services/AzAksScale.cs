using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksScale
{
    public AzAksScale(
        AzAksScaleAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksScaleAksPreview AksPreview { get; }
}