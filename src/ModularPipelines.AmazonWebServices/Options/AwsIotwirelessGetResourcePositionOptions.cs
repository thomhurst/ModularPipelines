using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "get-resource-position")]
public record AwsIotwirelessGetResourcePositionOptions(
[property: CommandSwitch("--resource-identifier")] string ResourceIdentifier,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AwsOptions;