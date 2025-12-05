using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "nodepool")]
public class AzAksNodepoolStop
{
    public AzAksNodepoolStop(
        AzAksNodepoolStopAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolStopAksPreview AksPreview { get; }
}