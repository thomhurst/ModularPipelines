using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
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