using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "delete-replication-group")]
public record AwsElasticacheDeleteReplicationGroupOptions(
[property: CommandSwitch("--replication-group-id")] string ReplicationGroupId
) : AwsOptions
{
    [CommandSwitch("--final-snapshot-identifier")]
    public string? FinalSnapshotIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}