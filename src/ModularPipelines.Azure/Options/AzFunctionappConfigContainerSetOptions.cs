using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "config", "container", "set")]
public record AzFunctionappConfigContainerSetOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CommandSwitch("--min-replicas")]
    public string? MinReplicas { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CommandSwitch("--registry-server")]
    public string? RegistryServer { get; set; }

    [CommandSwitch("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}