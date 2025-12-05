using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("grafana", "api-key", "create")]
public record AzGrafanaApiKeyCreateOptions(
[property: CliOption("--key")] string Key,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--time-to-live")]
    public string? TimeToLive { get; set; }
}