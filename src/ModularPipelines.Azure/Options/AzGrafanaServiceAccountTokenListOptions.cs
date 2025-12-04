using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("grafana", "service-account", "token", "list")]
public record AzGrafanaServiceAccountTokenListOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--service-account")] int ServiceAccount
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}