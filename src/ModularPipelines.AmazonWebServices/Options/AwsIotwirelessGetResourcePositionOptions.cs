using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "get-resource-position")]
public record AwsIotwirelessGetResourcePositionOptions(
[property: CliOption("--resource-identifier")] string ResourceIdentifier,
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions;