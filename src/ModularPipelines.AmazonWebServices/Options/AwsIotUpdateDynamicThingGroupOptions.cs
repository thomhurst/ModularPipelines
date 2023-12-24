using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-dynamic-thing-group")]
public record AwsIotUpdateDynamicThingGroupOptions(
[property: CommandSwitch("--thing-group-name")] string ThingGroupName,
[property: CommandSwitch("--thing-group-properties")] string ThingGroupProperties
) : AwsOptions
{
    [CommandSwitch("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CommandSwitch("--index-name")]
    public string? IndexName { get; set; }

    [CommandSwitch("--query-string")]
    public string? QueryString { get; set; }

    [CommandSwitch("--query-version")]
    public string? QueryVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}