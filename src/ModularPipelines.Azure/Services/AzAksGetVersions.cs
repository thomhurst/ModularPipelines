using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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