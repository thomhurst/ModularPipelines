using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "stop-speaker-search-task")]
public record AwsChimeSdkMediaPipelinesStopSpeakerSearchTaskOptions(
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--speaker-search-task-id")] string SpeakerSearchTaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}