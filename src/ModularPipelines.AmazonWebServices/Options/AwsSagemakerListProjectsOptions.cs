using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "list-projects")]
public record AwsSagemakerListProjectsOptions : AwsOptions
{
    [CliOption("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CliOption("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--name-contains")]
    public string? NameContains { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}