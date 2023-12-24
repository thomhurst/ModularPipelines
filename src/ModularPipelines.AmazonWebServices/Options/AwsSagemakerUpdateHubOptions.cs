using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-hub")]
public record AwsSagemakerUpdateHubOptions(
[property: CommandSwitch("--hub-name")] string HubName
) : AwsOptions
{
    [CommandSwitch("--hub-description")]
    public string? HubDescription { get; set; }

    [CommandSwitch("--hub-display-name")]
    public string? HubDisplayName { get; set; }

    [CommandSwitch("--hub-search-keywords")]
    public string[]? HubSearchKeywords { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}