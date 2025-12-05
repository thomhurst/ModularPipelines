using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "create-anomaly-monitor")]
public record AwsCeCreateAnomalyMonitorOptions(
[property: CliOption("--anomaly-monitor")] string AnomalyMonitor
) : AwsOptions
{
    [CliOption("--resource-tags")]
    public string[]? ResourceTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}