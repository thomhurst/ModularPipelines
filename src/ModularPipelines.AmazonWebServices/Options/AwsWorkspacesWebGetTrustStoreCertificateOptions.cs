using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "get-trust-store-certificate")]
public record AwsWorkspacesWebGetTrustStoreCertificateOptions(
[property: CliOption("--thumbprint")] string Thumbprint,
[property: CliOption("--trust-store-arn")] string TrustStoreArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}