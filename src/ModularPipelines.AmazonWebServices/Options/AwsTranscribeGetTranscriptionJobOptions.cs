using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "get-transcription-job")]
public record AwsTranscribeGetTranscriptionJobOptions(
[property: CliOption("--transcription-job-name")] string TranscriptionJobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}