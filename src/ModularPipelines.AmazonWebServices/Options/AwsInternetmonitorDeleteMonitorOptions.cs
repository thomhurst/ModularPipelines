using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("internetmonitor", "delete-monitor")]
public record AwsInternetmonitorDeleteMonitorOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}