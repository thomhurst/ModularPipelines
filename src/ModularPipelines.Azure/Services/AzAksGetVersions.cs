using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks")]
public class AzAksGetVersions
{
    public AzAksGetVersions(
        AzAksGetVersionsAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksGetVersionsAksPreview AksPreview { get; }
}