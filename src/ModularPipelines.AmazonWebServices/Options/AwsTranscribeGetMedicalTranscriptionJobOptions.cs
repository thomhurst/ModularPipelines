using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "get-medical-transcription-job")]
public record AwsTranscribeGetMedicalTranscriptionJobOptions(
[property: CliOption("--medical-transcription-job-name")] string MedicalTranscriptionJobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}