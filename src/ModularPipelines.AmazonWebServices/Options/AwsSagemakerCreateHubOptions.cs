using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-hub")]
public record AwsSagemakerCreateHubOptions(
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--hub-description")] string HubDescription
) : AwsOptions
{
    [CliOption("--hub-display-name")]
    public string? HubDisplayName { get; set; }

    [CliOption("--hub-search-keywords")]
    public string[]? HubSearchKeywords { get; set; }

    [CliOption("--s3-storage-config")]
    public string? S3StorageConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}