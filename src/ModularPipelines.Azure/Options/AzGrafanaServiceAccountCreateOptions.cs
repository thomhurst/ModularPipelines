using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "service-account", "create")]
public record AzGrafanaServiceAccountCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--service-account")] int ServiceAccount
) : AzOptions
{
    [BooleanCommandSwitch("--is-disabled")]
    public bool? IsDisabled { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }
}

