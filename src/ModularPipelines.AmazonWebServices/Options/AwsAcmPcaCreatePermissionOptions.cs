using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm-pca", "create-permission")]
public record AwsAcmPcaCreatePermissionOptions(
[property: CliOption("--certificate-authority-arn")] string CertificateAuthorityArn,
[property: CliOption("--principal")] string Principal,
[property: CliOption("--actions")] string[] Actions
) : AwsOptions
{
    [CliOption("--source-account")]
    public string? SourceAccount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}