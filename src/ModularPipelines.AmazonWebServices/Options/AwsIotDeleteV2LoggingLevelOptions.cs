using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "delete-v2-logging-level")]
public record AwsIotDeleteV2LoggingLevelOptions(
[property: CommandSwitch("--target-type")] string TargetType,
[property: CommandSwitch("--target-name")] string TargetName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}