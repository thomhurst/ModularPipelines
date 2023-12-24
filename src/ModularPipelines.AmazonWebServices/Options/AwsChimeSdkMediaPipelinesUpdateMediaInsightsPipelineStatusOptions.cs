using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "update-media-insights-pipeline-status")]
public record AwsChimeSdkMediaPipelinesUpdateMediaInsightsPipelineStatusOptions(
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--update-status")] string UpdateStatus
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}