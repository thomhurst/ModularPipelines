using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcdata", "ad-connector", "update")]
public record AzArcdataAdConnectorUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--data-controller-name")]
    public string? DataControllerName { get; set; }

    [CliOption("--dns-replicas")]
    public string? DnsReplicas { get; set; }

    [CliOption("--domain-service-account-secret")]
    public int? DomainServiceAccountSecret { get; set; }

    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliOption("--nameserver-addresses")]
    public string? NameserverAddresses { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--prefer-k8s-dns")]
    public string? PreferK8sDns { get; set; }

    [CliOption("--primary-ad-dc-hostname")]
    public string? PrimaryAdDcHostname { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secondary-ad-dc-hostnames")]
    public string? SecondaryAdDcHostnames { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}