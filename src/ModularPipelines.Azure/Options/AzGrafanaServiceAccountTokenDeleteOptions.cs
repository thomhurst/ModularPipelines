using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("grafana", "service-account", "token", "delete")]
public record AzGrafanaServiceAccountTokenDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--service-account")] int ServiceAccount,
[property: CliOption("--token")] string Token
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}