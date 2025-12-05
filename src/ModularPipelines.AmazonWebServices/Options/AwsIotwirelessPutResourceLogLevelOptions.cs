using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "put-resource-log-level")]
public record AwsIotwirelessPutResourceLogLevelOptions(
[property: CliOption("--resource-identifier")] string ResourceIdentifier,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--log-level")] string LogLevel
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}