using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "data-source", "query")]
public record AzGrafanaDataSourceQueryOptions(
[property: CommandSwitch("--data-source")] string DataSource,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--api-key")]
    public string? ApiKey { get; set; }

    [CommandSwitch("--conditions")]
    public string? Conditions { get; set; }

    [CommandSwitch("--from")]
    public string? From { get; set; }

    [CommandSwitch("--internal-ms")]
    public string? InternalMs { get; set; }

    [CommandSwitch("--max-data-points")]
    public string? MaxDataPoints { get; set; }

    [CommandSwitch("--query-format")]
    public string? QueryFormat { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--to")]
    public string? To { get; set; }
}