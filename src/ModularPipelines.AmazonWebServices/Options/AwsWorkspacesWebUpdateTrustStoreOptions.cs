using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "update-trust-store")]
public record AwsWorkspacesWebUpdateTrustStoreOptions(
[property: CliOption("--trust-store-arn")] string TrustStoreArn
) : AwsOptions
{
    [CliOption("--certificates-to-add")]
    public string[]? CertificatesToAdd { get; set; }

    [CliOption("--certificates-to-delete")]
    public string[]? CertificatesToDelete { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}