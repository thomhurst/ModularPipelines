using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "create-custom-domain-association")]
public record AwsRedshiftCreateCustomDomainAssociationOptions(
[property: CliOption("--custom-domain-name")] string CustomDomainName,
[property: CliOption("--custom-domain-certificate-arn")] string CustomDomainCertificateArn,
[property: CliOption("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}