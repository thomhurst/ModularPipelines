using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedk8s", "upgrade")]
public record AzConnectedk8sUpgradeOptions : AzOptions
{
    [CommandSwitch("--agent-version")]
    public string? AgentVersion { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kube-config")]
    public string? KubeConfig { get; set; }

    [CommandSwitch("--kube-context")]
    public string? KubeContext { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--upgrade-timeout")]
    public string? UpgradeTimeout { get; set; }
}

