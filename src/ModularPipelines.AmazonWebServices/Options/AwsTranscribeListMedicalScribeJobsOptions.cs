using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcribe", "list-medical-scribe-jobs")]
public record AwsTranscribeListMedicalScribeJobsOptions : AwsOptions
{
    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--job-name-contains")]
    public string? JobNameContains { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}