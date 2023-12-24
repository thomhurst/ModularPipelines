using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "delete-replication-configuration-template")]
public record AwsMgnDeleteReplicationConfigurationTemplateOptions(
[property: CommandSwitch("--replication-configuration-template-id")] string ReplicationConfigurationTemplateId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}