using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksStart
{
    public AzAksStart(
        AzAksStartAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksStartAksPreview AksPreview { get; }
}