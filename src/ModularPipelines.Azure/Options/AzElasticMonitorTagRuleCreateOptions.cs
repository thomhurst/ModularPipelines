using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic", "monitor", "tag-rule", "create")]
public record AzElasticMonitorTagRuleCreateOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--log-rules")]
    public string? LogRules { get; set; }
}