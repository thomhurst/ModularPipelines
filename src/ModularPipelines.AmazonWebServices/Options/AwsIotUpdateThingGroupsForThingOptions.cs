using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-thing-groups-for-thing")]
public record AwsIotUpdateThingGroupsForThingOptions : AwsOptions
{
    [CommandSwitch("--thing-name")]
    public string? ThingName { get; set; }

    [CommandSwitch("--thing-groups-to-add")]
    public string[]? ThingGroupsToAdd { get; set; }

    [CommandSwitch("--thing-groups-to-remove")]
    public string[]? ThingGroupsToRemove { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}