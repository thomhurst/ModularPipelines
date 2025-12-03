using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-hub")]
public record AwsSagemakerUpdateHubOptions(
[property: CliOption("--hub-name")] string HubName
) : AwsOptions
{
    [CliOption("--hub-description")]
    public string? HubDescription { get; set; }

    [CliOption("--hub-display-name")]
    public string? HubDisplayName { get; set; }

    [CliOption("--hub-search-keywords")]
    public string[]? HubSearchKeywords { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}