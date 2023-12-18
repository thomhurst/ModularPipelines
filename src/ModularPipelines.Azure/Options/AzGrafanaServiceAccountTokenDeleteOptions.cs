using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "service-account", "token", "delete")]
public record AzGrafanaServiceAccountTokenDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--service-account")] int ServiceAccount,
[property: CommandSwitch("--token")] string Token
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}