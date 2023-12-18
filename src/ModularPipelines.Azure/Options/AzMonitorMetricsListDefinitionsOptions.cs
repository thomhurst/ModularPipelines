using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "metrics", "list-definitions")]
public record AzMonitorMetricsListDefinitionsOptions(
[property: CommandSwitch("--resource")] string Resource
) : AzOptions
{
    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-namespace")]
    public string? ResourceNamespace { get; set; }

    [CommandSwitch("--resource-parent")]
    public string? ResourceParent { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }
}