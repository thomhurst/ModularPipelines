using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-media-pipelines", "get-speaker-search-task")]
public record AwsChimeSdkMediaPipelinesGetSpeakerSearchTaskOptions(
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--speaker-search-task-id")] string SpeakerSearchTaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}