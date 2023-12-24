using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "put-targets")]
public record AwsEventsPutTargetsOptions(
[property: CommandSwitch("--rule")] string Rule,
[property: CommandSwitch("--targets")] string[] Targets
) : AwsOptions
{
    [CommandSwitch("--event-bus-name")]
    public string? EventBusName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}