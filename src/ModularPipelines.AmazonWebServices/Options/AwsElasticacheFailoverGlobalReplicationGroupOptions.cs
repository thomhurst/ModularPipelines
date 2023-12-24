using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "failover-global-replication-group")]
public record AwsElasticacheFailoverGlobalReplicationGroupOptions(
[property: CommandSwitch("--global-replication-group-id")] string GlobalReplicationGroupId,
[property: CommandSwitch("--primary-region")] string PrimaryRegion,
[property: CommandSwitch("--primary-replication-group-id")] string PrimaryReplicationGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}