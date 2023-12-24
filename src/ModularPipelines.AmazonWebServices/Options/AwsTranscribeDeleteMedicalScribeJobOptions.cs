using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcribe", "delete-medical-scribe-job")]
public record AwsTranscribeDeleteMedicalScribeJobOptions(
[property: CommandSwitch("--medical-scribe-job-name")] string MedicalScribeJobName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}