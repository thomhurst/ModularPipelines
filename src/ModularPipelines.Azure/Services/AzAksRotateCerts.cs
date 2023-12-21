using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksRotateCerts
{
    public AzAksRotateCerts(
        AzAksRotateCertsAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksRotateCertsAksPreview AksPreview { get; }
}