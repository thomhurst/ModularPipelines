using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "authorize-snapshot-access")]
public record AwsRedshiftAuthorizeSnapshotAccessOptions(
[property: CliOption("--account-with-restore-access")] string AccountWithRestoreAccess
) : AwsOptions
{
    [CliOption("--snapshot-identifier")]
    public string? SnapshotIdentifier { get; set; }

    [CliOption("--snapshot-arn")]
    public string? SnapshotArn { get; set; }

    [CliOption("--snapshot-cluster-identifier")]
    public string? SnapshotClusterIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}