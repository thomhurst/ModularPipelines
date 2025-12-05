using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "describe-node-configuration-options")]
public record AwsRedshiftDescribeNodeConfigurationOptionsOptions(
[property: CliOption("--action-type")] string ActionType
) : AwsOptions
{
    [CliOption("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CliOption("--snapshot-identifier")]
    public string? SnapshotIdentifier { get; set; }

    [CliOption("--snapshot-arn")]
    public string? SnapshotArn { get; set; }

    [CliOption("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}