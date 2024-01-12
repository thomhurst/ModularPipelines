using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "domain-mappings", "describe")]
public record GcloudRunDomainMappingsDescribeOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--namespace")] string Namespace
) : GcloudOptions
{
    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--cluster-location")]
    public string? ClusterLocation { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--kubeconfig")]
    public string? Kubeconfig { get; set; }
}