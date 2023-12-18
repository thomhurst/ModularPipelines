using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "service-account", "update")]
public record AzGrafanaServiceAccountUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--service-account")] int ServiceAccount
) : AzOptions
{
    [BooleanCommandSwitch("--is-disabled")]
    public bool? IsDisabled { get; set; }

    [CommandSwitch("--new-name")]
    public string? NewName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }
}