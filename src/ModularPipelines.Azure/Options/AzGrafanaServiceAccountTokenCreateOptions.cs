using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("grafana", "service-account", "token", "create")]
public record AzGrafanaServiceAccountTokenCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--service-account")] int ServiceAccount,
[property: CliOption("--token")] string Token
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--time-to-live")]
    public string? TimeToLive { get; set; }
}