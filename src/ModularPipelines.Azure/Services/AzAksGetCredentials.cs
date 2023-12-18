using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

