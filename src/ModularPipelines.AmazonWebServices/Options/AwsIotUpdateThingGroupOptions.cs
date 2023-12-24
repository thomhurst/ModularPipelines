using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-thing-group")]
public record AwsIotUpdateThingGroupOptions(
[property: CommandSwitch("--thing-group-name")] string ThingGroupName,
[property: CommandSwitch("--thing-group-properties")] string ThingGroupProperties
) : AwsOptions
{
    [CommandSwitch("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}