using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "describe-partners")]
public record AwsRedshiftDescribePartnersOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--partner-name")]
    public string? PartnerName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}