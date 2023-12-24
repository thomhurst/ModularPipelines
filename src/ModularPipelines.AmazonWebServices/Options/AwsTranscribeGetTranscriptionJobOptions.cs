using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "get-transcription-job")]
public record AwsTranscribeGetTranscriptionJobOptions(
[property: CommandSwitch("--transcription-job-name")] string TranscriptionJobName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}