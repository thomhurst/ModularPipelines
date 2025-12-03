using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("grafana", "service-account", "delete")]
public record AzGrafanaServiceAccountDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--service-account")] int ServiceAccount
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}