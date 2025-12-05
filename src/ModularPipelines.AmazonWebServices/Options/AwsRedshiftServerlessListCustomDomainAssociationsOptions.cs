using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "list-custom-domain-associations")]
public record AwsRedshiftServerlessListCustomDomainAssociationsOptions : AwsOptions
{
    [CliOption("--custom-domain-certificate-arn")]
    public string? CustomDomainCertificateArn { get; set; }

    [CliOption("--custom-domain-name")]
    public string? CustomDomainName { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}