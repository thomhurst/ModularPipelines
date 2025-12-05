using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "list-image-versions")]
public record AwsSagemakerListImageVersionsOptions(
[property: CliOption("--image-name")] string ImageName
) : AwsOptions
{
    [CliOption("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CliOption("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CliOption("--last-modified-time-after")]
    public long? LastModifiedTimeAfter { get; set; }

    [CliOption("--last-modified-time-before")]
    public long? LastModifiedTimeBefore { get; set; }

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