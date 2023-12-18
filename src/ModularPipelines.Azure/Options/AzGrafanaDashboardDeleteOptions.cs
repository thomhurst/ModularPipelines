using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "dashboard", "delete")]
public record AzGrafanaDashboardDeleteOptions(
[property: CommandSwitch("--dashboard")] string Dashboard,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--api-key")]
    public string? ApiKey { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}