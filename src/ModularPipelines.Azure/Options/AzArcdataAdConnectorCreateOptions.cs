using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "ad-connector", "create")]
public record AzArcdataAdConnectorCreateOptions(
[property: CommandSwitch("--account-provisioning")] int AccountProvisioning,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--nameserver-addresses")] string NameserverAddresses,
[property: CommandSwitch("--realm")] string Realm
) : AzOptions
{
    [CommandSwitch("--data-controller-name")]
    public string? DataControllerName { get; set; }

    [CommandSwitch("--dns-domain-name")]
    public string? DnsDomainName { get; set; }

    [CommandSwitch("--dns-replicas")]
    public string? DnsReplicas { get; set; }

    [CommandSwitch("--domain-service-account-secret")]
    public int? DomainServiceAccountSecret { get; set; }

    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CommandSwitch("--netbios-domain-name")]
    public string? NetbiosDomainName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--ou-distinguished-name")]
    public string? OuDistinguishedName { get; set; }

    [BooleanCommandSwitch("--prefer-k8s-dns")]
    public bool? PreferK8sDns { get; set; }

    [CommandSwitch("--primary-ad-dc-hostname")]
    public string? PrimaryAdDcHostname { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secondary-ad-dc-hostnames")]
    public string? SecondaryAdDcHostnames { get; set; }

    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}