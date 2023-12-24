using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "get-medical-transcription-job")]
public record AwsTranscribeGetMedicalTranscriptionJobOptions(
[property: CommandSwitch("--medical-transcription-job-name")] string MedicalTranscriptionJobName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}