using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "create-global-replication-group")]
public record AwsElasticacheCreateGlobalReplicationGroupOptions(
[property: CommandSwitch("--global-replication-group-id-suffix")] string GlobalReplicationGroupIdSuffix,
[property: CommandSwitch("--primary-replication-group-id")] string PrimaryReplicationGroupId
) : AwsOptions
{
    [CommandSwitch("--global-replication-group-description")]
    public string? GlobalReplicationGroupDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}