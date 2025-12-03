using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastic", "monitor", "tag-rule", "create")]
public record AzElasticMonitorTagRuleCreateOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--log-rules")]
    public string? LogRules { get; set; }
}