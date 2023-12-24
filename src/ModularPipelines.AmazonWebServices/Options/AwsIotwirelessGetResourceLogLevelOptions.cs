using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "get-resource-log-level")]
public record AwsIotwirelessGetResourceLogLevelOptions(
[property: CommandSwitch("--resource-identifier")] string ResourceIdentifier,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}