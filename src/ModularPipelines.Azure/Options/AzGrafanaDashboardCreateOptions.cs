using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("grafana", "dashboard", "create")]
public record AzGrafanaDashboardCreateOptions(
[property: CliOption("--definition")] string Definition,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--api-key")]
    public string? ApiKey { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliFlag("--overwrite")]
    public bool? Overwrite { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }
}