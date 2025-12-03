using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectedk8s", "upgrade")]
public record AzConnectedk8sUpgradeOptions : AzOptions
{
    [CliOption("--agent-version")]
    public string? AgentVersion { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kube-config")]
    public string? KubeConfig { get; set; }

    [CliOption("--kube-context")]
    public string? KubeContext { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--upgrade-timeout")]
    public string? UpgradeTimeout { get; set; }
}