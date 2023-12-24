using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents", "create-input")]
public record AwsIoteventsCreateInputOptions(
[property: CommandSwitch("--input-name")] string InputName,
[property: CommandSwitch("--input-definition")] string InputDefinition
) : AwsOptions
{
    [CommandSwitch("--input-description")]
    public string? InputDescription { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}