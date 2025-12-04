using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("grafana", "service-account", "update")]
public record AzGrafanaServiceAccountUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--service-account")] int ServiceAccount
) : AzOptions
{
    [CliFlag("--is-disabled")]
    public bool? IsDisabled { get; set; }

    [CliOption("--new-name")]
    public string? NewName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }
}