using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "list-candidates-for-auto-ml-job")]
public record AwsSagemakerListCandidatesForAutoMlJobOptions(
[property: CliOption("--auto-ml-job-name")] string AutoMlJobName
) : AwsOptions
{
    [CliOption("--status-equals")]
    public string? StatusEquals { get; set; }

    [CliOption("--candidate-name-equals")]
    public string? CandidateNameEquals { get; set; }

    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}