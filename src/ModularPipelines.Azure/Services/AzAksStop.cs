using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks")]
public class AzAksStop
{
    public AzAksStop(
        AzAksStopAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksStopAksPreview AksPreview { get; }
}