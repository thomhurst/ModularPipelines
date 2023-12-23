using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-hub-content")]
public record AwsSagemakerDescribeHubContentOptions(
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--hub-content-type")] string HubContentType,
[property: CommandSwitch("--hub-content-name")] string HubContentName
) : AwsOptions
{
    [CommandSwitch("--hub-content-version")]
    public string? HubContentVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}