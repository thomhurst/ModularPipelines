using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksGetCredentials
{
    public AzAksGetCredentials(
        AzAksGetCredentialsAksPreview aksPreview
    )
    {
        AksPreview = aksPreview;
    }

    public AzAksGetCredentialsAksPreview AksPreview { get; }
}