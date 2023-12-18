using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "ad-connector", "update")]
public record AzArcdataAdConnectorUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--data-controller-name")]
    public string? DataControllerName { get; set; }

    [CommandSwitch("--dns-replicas")]
    public string? DnsReplicas { get; set; }

    [CommandSwitch("--domain-service-account-secret")]
    public int? DomainServiceAccountSecret { get; set; }

    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CommandSwitch("--nameserver-addresses")]
    public string? NameserverAddresses { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--prefer-k8s-dns")]
    public string? PreferK8sDns { get; set; }

    [CommandSwitch("--primary-ad-dc-hostname")]
    public string? PrimaryAdDcHostname { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secondary-ad-dc-hostnames")]
    public string? SecondaryAdDcHostnames { get; set; }

    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}