using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "create-anomaly-monitor")]
public record AwsCeCreateAnomalyMonitorOptions(
[property: CommandSwitch("--anomaly-monitor")] string AnomalyMonitor
) : AwsOptions
{
    [CommandSwitch("--resource-tags")]
    public string[]? ResourceTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}