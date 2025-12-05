using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "get-medical-scribe-job")]
public record AwsTranscribeGetMedicalScribeJobOptions(
[property: CliOption("--medical-scribe-job-name")] string MedicalScribeJobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}