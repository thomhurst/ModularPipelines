using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("grafana", "dashboard", "delete")]
public record AzGrafanaDashboardDeleteOptions(
[property: CliOption("--dashboard")] string Dashboard,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--api-key")]
    public string? ApiKey { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}