using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "modify-global-replication-group")]
public record AwsElasticacheModifyGlobalReplicationGroupOptions(
[property: CommandSwitch("--global-replication-group-id")] string GlobalReplicationGroupId
) : AwsOptions
{
    [CommandSwitch("--cache-node-type")]
    public string? CacheNodeType { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--cache-parameter-group-name")]
    public string? CacheParameterGroupName { get; set; }

    [CommandSwitch("--global-replication-group-description")]
    public string? GlobalReplicationGroupDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}