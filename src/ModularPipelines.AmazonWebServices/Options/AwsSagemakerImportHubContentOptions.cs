using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "import-hub-content")]
public record AwsSagemakerImportHubContentOptions(
[property: CliOption("--hub-content-name")] string HubContentName,
[property: CliOption("--hub-content-type")] string HubContentType,
[property: CliOption("--document-schema-version")] string DocumentSchemaVersion,
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--hub-content-document")] string HubContentDocument
) : AwsOptions
{
    [CliOption("--hub-content-version")]
    public string? HubContentVersion { get; set; }

    [CliOption("--hub-content-display-name")]
    public string? HubContentDisplayName { get; set; }

    [CliOption("--hub-content-description")]
    public string? HubContentDescription { get; set; }

    [CliOption("--hub-content-markdown")]
    public string? HubContentMarkdown { get; set; }

    [CliOption("--hub-content-search-keywords")]
    public string[]? HubContentSearchKeywords { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}