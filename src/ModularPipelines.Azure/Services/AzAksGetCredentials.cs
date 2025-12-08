using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks")]
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