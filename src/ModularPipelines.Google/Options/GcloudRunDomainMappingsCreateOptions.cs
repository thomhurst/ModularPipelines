using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "domain-mappings", "create")]
public record GcloudRunDomainMappingsCreateOptions(
[property: CliOption("--service")] string Service,
[property: CliOption("--domain")] string Domain,
[property: CliOption("--namespace")] string Namespace
) : GcloudOptions
{
    [CliFlag("--force-override")]
    public bool? ForceOverride { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--cluster-location")]
    public string? ClusterLocation { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--kubeconfig")]
    public string? Kubeconfig { get; set; }
}