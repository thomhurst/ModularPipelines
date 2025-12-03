using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "create-custom-domain-association")]
public record AwsRedshiftServerlessCreateCustomDomainAssociationOptions(
[property: CliOption("--custom-domain-certificate-arn")] string CustomDomainCertificateArn,
[property: CliOption("--custom-domain-name")] string CustomDomainName,
[property: CliOption("--workgroup-name")] string WorkgroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}