using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm", "update-certificate-options")]
public record AwsAcmUpdateCertificateOptionsOptions(
[property: CliOption("--certificate-arn")] string CertificateArn,
[property: CliOption("--options")] string Options
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}