using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-dynamic-thing-group")]
public record AwsIotCreateDynamicThingGroupOptions(
[property: CommandSwitch("--thing-group-name")] string ThingGroupName,
[property: CommandSwitch("--query-string")] string QueryString
) : AwsOptions
{
    [CommandSwitch("--thing-group-properties")]
    public string? ThingGroupProperties { get; set; }

    [CommandSwitch("--index-name")]
    public string? IndexName { get; set; }

    [CommandSwitch("--query-version")]
    public string? QueryVersion { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}