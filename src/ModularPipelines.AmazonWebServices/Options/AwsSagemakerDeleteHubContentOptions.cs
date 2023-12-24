using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "delete-hub-content")]
public record AwsSagemakerDeleteHubContentOptions(
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--hub-content-type")] string HubContentType,
[property: CommandSwitch("--hub-content-name")] string HubContentName,
[property: CommandSwitch("--hub-content-version")] string HubContentVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}