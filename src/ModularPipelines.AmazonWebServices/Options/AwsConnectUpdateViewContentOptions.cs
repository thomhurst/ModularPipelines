using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-view-content")]
public record AwsConnectUpdateViewContentOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--view-id")] string ViewId,
[property: CommandSwitch("--status")] string Status,
[property: CommandSwitch("--content")] string Content
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}