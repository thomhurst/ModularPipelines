using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "disassociate-global-replication-group")]
public record AwsElasticacheDisassociateGlobalReplicationGroupOptions(
[property: CommandSwitch("--global-replication-group-id")] string GlobalReplicationGroupId,
[property: CommandSwitch("--replication-group-id")] string ReplicationGroupId,
[property: CommandSwitch("--replication-group-region")] string ReplicationGroupRegion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}