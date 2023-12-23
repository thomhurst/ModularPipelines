using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "import-hub-content")]
public record AwsSagemakerImportHubContentOptions(
[property: CommandSwitch("--hub-content-name")] string HubContentName,
[property: CommandSwitch("--hub-content-type")] string HubContentType,
[property: CommandSwitch("--document-schema-version")] string DocumentSchemaVersion,
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--hub-content-document")] string HubContentDocument
) : AwsOptions
{
    [CommandSwitch("--hub-content-version")]
    public string? HubContentVersion { get; set; }

    [CommandSwitch("--hub-content-display-name")]
    public string? HubContentDisplayName { get; set; }

    [CommandSwitch("--hub-content-description")]
    public string? HubContentDescription { get; set; }

    [CommandSwitch("--hub-content-markdown")]
    public string? HubContentMarkdown { get; set; }

    [CommandSwitch("--hub-content-search-keywords")]
    public string[]? HubContentSearchKeywords { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}