using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "remove-targets")]
public record AwsEventsRemoveTargetsOptions(
[property: CommandSwitch("--rule")] string Rule,
[property: CommandSwitch("--ids")] string[] Ids
) : AwsOptions
{
    [CommandSwitch("--event-bus-name")]
    public string? EventBusName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}