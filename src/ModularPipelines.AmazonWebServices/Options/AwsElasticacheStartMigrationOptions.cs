using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "start-migration")]
public record AwsElasticacheStartMigrationOptions(
[property: CommandSwitch("--replication-group-id")] string ReplicationGroupId,
[property: CommandSwitch("--customer-node-endpoint-list")] string[] CustomerNodeEndpointList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}