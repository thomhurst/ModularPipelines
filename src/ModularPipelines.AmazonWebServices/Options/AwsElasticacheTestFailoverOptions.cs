using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "test-failover")]
public record AwsElasticacheTestFailoverOptions(
[property: CommandSwitch("--replication-group-id")] string ReplicationGroupId,
[property: CommandSwitch("--node-group-id")] string NodeGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}