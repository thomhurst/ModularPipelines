using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotanalytics", "put-logging-options")]
public record AwsIotanalyticsPutLoggingOptionsOptions(
[property: CommandSwitch("--logging-options")] string LoggingOptions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}