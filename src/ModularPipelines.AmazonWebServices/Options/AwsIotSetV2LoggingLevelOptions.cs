using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "set-v2-logging-level")]
public record AwsIotSetV2LoggingLevelOptions(
[property: CommandSwitch("--log-target")] string LogTarget,
[property: CommandSwitch("--log-level")] string LogLevel
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}