using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "update-partner-status")]
public record AwsRedshiftUpdatePartnerStatusOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--cluster-identifier")] string ClusterIdentifier,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--partner-name")] string PartnerName,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--status-message")]
    public string? StatusMessage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}