using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "delete-cluster-snapshot")]
public record AwsRedshiftDeleteClusterSnapshotOptions(
[property: CommandSwitch("--snapshot-identifier")] string SnapshotIdentifier
) : AwsOptions
{
    [CommandSwitch("--snapshot-cluster-identifier")]
    public string? SnapshotClusterIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}