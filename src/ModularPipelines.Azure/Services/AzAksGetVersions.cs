using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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