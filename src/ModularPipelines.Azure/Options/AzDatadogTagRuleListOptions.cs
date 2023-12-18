using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datadog", "tag-rule", "list")]
public record AzDatadogTagRuleListOptions(
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;