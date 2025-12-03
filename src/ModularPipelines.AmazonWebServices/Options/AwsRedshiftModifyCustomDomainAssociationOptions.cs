using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "modify-custom-domain-association")]
public record AwsRedshiftModifyCustomDomainAssociationOptions(
[property: CliOption("--custom-domain-name")] string CustomDomainName,
[property: CliOption("--custom-domain-certificate-arn")] string CustomDomainCertificateArn,
[property: CliOption("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}