using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("aks")]
public class AzAksWait
{
    public AzAksWait(
        AzAksWaitAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksWaitAksPreview AksPreview { get; }
}