using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "put-replication-configuration")]
public record AwsEcrPutReplicationConfigurationOptions(
[property: CommandSwitch("--replication-configuration")] string ReplicationConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}