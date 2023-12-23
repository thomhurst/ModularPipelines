using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-thing-type")]
public record AwsIotCreateThingTypeOptions(
[property: CommandSwitch("--thing-type-name")] string ThingTypeName
) : AwsOptions
{
    [CommandSwitch("--thing-type-properties")]
    public string? ThingTypeProperties { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}