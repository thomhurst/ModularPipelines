using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "set-v2-logging-level")]
public record AwsIotSetV2LoggingLevelOptions(
[property: CliOption("--log-target")] string LogTarget,
[property: CliOption("--log-level")] string LogLevel
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}