using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces-web", "get-trust-store-certificate")]
public record AwsWorkspacesWebGetTrustStoreCertificateOptions(
[property: CommandSwitch("--thumbprint")] string Thumbprint,
[property: CommandSwitch("--trust-store-arn")] string TrustStoreArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}