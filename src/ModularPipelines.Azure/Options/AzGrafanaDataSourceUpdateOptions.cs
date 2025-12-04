using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("grafana", "data-source", "update")]
public record AzGrafanaDataSourceUpdateOptions(
[property: CliOption("--data-source")] string DataSource,
[property: CliOption("--definition")] string Definition,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--api-key")]
    public string? ApiKey { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}