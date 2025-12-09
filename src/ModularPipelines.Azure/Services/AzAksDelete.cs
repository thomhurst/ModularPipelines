using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks")]
public class AzAksDelete
{
    public AzAksDelete(
        AzAksDeleteAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksDeleteAksPreview AksPreview { get; }
}