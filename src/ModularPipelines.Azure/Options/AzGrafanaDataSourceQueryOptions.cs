using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("grafana", "data-source", "query")]
public record AzGrafanaDataSourceQueryOptions(
[property: CliOption("--data-source")] string DataSource,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--api-key")]
    public string? ApiKey { get; set; }

    [CliOption("--conditions")]
    public string? Conditions { get; set; }

    [CliOption("--from")]
    public string? From { get; set; }

    [CliOption("--internal-ms")]
    public string? InternalMs { get; set; }

    [CliOption("--max-data-points")]
    public string? MaxDataPoints { get; set; }

    [CliOption("--query-format")]
    public string? QueryFormat { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--to")]
    public string? To { get; set; }
}