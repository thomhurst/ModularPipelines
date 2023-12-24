using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("drs", "delete-replication-configuration-template")]
public record AwsDrsDeleteReplicationConfigurationTemplateOptions(
[property: CommandSwitch("--replication-configuration-template-id")] string ReplicationConfigurationTemplateId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}