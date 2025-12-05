using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "nodepool")]
public class AzAksNodepoolOperationAbort
{
    public AzAksNodepoolOperationAbort(
        AzAksNodepoolOperationAbortAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksNodepoolOperationAbortAksPreview AksPreview { get; }
}