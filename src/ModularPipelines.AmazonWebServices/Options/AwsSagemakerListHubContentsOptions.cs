using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "list-hub-contents")]
public record AwsSagemakerListHubContentsOptions(
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--hub-content-type")] string HubContentType
) : AwsOptions
{
    [CliOption("--name-contains")]
    public string? NameContains { get; set; }

    [CliOption("--max-schema-version")]
    public string? MaxSchemaVersion { get; set; }

    [CliOption("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CliOption("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}