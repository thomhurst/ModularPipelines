using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastictranscoder", "update-pipeline-notifications")]
public record AwsElastictranscoderUpdatePipelineNotificationsOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--notifications")] string Notifications
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}