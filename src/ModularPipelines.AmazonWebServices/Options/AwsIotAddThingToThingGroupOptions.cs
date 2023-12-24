using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "add-thing-to-thing-group")]
public record AwsIotAddThingToThingGroupOptions : AwsOptions
{
    [CommandSwitch("--thing-group-name")]
    public string? ThingGroupName { get; set; }

    [CommandSwitch("--thing-group-arn")]
    public string? ThingGroupArn { get; set; }

    [CommandSwitch("--thing-name")]
    public string? ThingName { get; set; }

    [CommandSwitch("--thing-arn")]
    public string? ThingArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}