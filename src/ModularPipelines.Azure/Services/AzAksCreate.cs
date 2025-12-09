using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks")]
public class AzAksCreate
{
    public AzAksCreate(
        AzAksCreateAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksCreateAksPreview AksPreview { get; }
}