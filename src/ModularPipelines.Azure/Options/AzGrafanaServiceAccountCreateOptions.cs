using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("grafana", "service-account", "create")]
public record AzGrafanaServiceAccountCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--service-account")] int ServiceAccount
) : AzOptions
{
    [CliFlag("--is-disabled")]
    public bool? IsDisabled { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }
}