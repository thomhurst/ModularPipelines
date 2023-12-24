using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents", "update-input")]
public record AwsIoteventsUpdateInputOptions(
[property: CommandSwitch("--input-name")] string InputName,
[property: CommandSwitch("--input-definition")] string InputDefinition
) : AwsOptions
{
    [CommandSwitch("--input-description")]
    public string? InputDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}