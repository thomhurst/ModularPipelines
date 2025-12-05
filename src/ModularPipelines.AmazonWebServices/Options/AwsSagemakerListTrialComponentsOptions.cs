using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "list-trial-components")]
public record AwsSagemakerListTrialComponentsOptions : AwsOptions
{
    [CliOption("--experiment-name")]
    public string? ExperimentName { get; set; }

    [CliOption("--trial-name")]
    public string? TrialName { get; set; }

    [CliOption("--source-arn")]
    public string? SourceArn { get; set; }

    [CliOption("--created-after")]
    public long? CreatedAfter { get; set; }

    [CliOption("--created-before")]
    public long? CreatedBefore { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}