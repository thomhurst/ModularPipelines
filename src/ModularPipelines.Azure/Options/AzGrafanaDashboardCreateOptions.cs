using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "dashboard", "create")]
public record AzGrafanaDashboardCreateOptions(
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

    [CommandSwitch("--title")]
    public string? Title { get; set; }
}