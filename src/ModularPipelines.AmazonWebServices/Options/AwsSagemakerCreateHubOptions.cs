using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-hub")]
public record AwsSagemakerCreateHubOptions(
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--hub-description")] string HubDescription
) : AwsOptions
{
    [CommandSwitch("--hub-display-name")]
    public string? HubDisplayName { get; set; }

    [CommandSwitch("--hub-search-keywords")]
    public string[]? HubSearchKeywords { get; set; }

    [CommandSwitch("--s3-storage-config")]
    public string? S3StorageConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}