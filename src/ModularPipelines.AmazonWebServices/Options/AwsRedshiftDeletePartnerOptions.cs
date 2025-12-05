using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "delete-partner")]
public record AwsRedshiftDeletePartnerOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--cluster-identifier")] string ClusterIdentifier,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--partner-name")] string PartnerName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}