using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "dashboard", "update")]
public record AzGrafanaDashboardUpdateOptions(
[property: CommandSwitch("--definition")] string Definition,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--api-key")]
    public string? ApiKey { get; set; }

    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public bool? Overwrite { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}