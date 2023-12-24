using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-media-pipelines", "get-speaker-search-task")]
public record AwsChimeSdkMediaPipelinesGetSpeakerSearchTaskOptions(
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--speaker-search-task-id")] string SpeakerSearchTaskId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}