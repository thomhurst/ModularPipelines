using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksUpdate
{
    public AzAksUpdate(
        AzAksUpdateAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksUpdateAksPreview AksPreview { get; }
}