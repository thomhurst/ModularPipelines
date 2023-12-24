using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("drs", "put-launch-action")]
public record AwsDrsPutLaunchActionOptions(
[property: CommandSwitch("--action-code")] string ActionCode,
[property: CommandSwitch("--action-id")] string ActionId,
[property: CommandSwitch("--action-version")] string ActionVersion,
[property: CommandSwitch("--category")] string Category,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--order")] int Order,
[property: CommandSwitch("--resource-id")] string ResourceId
) : AwsOptions
{
    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}