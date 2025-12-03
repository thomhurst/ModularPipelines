using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "delete-medical-transcription-job")]
public record AwsTranscribeDeleteMedicalTranscriptionJobOptions(
[property: CliOption("--medical-transcription-job-name")] string MedicalTranscriptionJobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}