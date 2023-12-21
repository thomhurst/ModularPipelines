using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
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