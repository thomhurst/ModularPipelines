using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "update-media-insights-pipeline-status")]
public record AwsChimeSdkMediaPipelinesUpdateMediaInsightsPipelineStatusOptions(
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--update-status")] string UpdateStatus
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}