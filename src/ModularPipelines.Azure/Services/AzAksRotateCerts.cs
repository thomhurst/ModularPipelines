using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks")]
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