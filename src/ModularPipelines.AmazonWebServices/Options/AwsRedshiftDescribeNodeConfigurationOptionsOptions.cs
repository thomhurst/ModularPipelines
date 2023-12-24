using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "describe-node-configuration-options")]
public record AwsRedshiftDescribeNodeConfigurationOptionsOptions(
[property: CommandSwitch("--action-type")] string ActionType
) : AwsOptions
{
    [CommandSwitch("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CommandSwitch("--snapshot-identifier")]
    public string? SnapshotIdentifier { get; set; }

    [CommandSwitch("--snapshot-arn")]
    public string? SnapshotArn { get; set; }

    [CommandSwitch("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}