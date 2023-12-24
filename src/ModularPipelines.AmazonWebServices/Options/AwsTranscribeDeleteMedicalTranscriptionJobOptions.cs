using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "delete-medical-transcription-job")]
public record AwsTranscribeDeleteMedicalTranscriptionJobOptions(
[property: CommandSwitch("--medical-transcription-job-name")] string MedicalTranscriptionJobName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}