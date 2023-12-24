using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "put-resource-log-level")]
public record AwsIotwirelessPutResourceLogLevelOptions(
[property: CommandSwitch("--resource-identifier")] string ResourceIdentifier,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--log-level")] string LogLevel
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}