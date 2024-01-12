using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "domain-mappings", "create")]
public record GcloudRunDomainMappingsCreateOptions(
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--namespace")] string Namespace
) : GcloudOptions
{
    [BooleanCommandSwitch("--force-override")]
    public bool? ForceOverride { get; set; }

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