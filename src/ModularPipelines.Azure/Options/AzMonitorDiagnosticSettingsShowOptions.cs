using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "diagnostic-settings", "show")]
public record AzMonitorDiagnosticSettingsShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource")] string Resource
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-namespace")]
    public string? ResourceNamespace { get; set; }

    [CommandSwitch("--resource-parent")]
    public string? ResourceParent { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }
}