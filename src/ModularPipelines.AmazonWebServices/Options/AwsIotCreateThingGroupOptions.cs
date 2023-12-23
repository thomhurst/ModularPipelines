using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-thing-group")]
public record AwsIotCreateThingGroupOptions(
[property: CommandSwitch("--thing-group-name")] string ThingGroupName
) : AwsOptions
{
    [CommandSwitch("--parent-group-name")]
    public string? ParentGroupName { get; set; }

    [CommandSwitch("--thing-group-properties")]
    public string? ThingGroupProperties { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}