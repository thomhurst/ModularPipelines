using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "revoke-snapshot-access")]
public record AwsRedshiftRevokeSnapshotAccessOptions(
[property: CommandSwitch("--account-with-restore-access")] string AccountWithRestoreAccess
) : AwsOptions
{
    [CommandSwitch("--snapshot-identifier")]
    public string? SnapshotIdentifier { get; set; }

    [CommandSwitch("--snapshot-arn")]
    public string? SnapshotArn { get; set; }

    [CommandSwitch("--snapshot-cluster-identifier")]
    public string? SnapshotClusterIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}