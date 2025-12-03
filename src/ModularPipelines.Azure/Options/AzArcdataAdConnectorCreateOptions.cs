using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcdata", "ad-connector", "create")]
public record AzArcdataAdConnectorCreateOptions(
[property: CliOption("--account-provisioning")] int AccountProvisioning,
[property: CliOption("--name")] string Name,
[property: CliOption("--nameserver-addresses")] string NameserverAddresses,
[property: CliOption("--realm")] string Realm
) : AzOptions
{
    [CliOption("--data-controller-name")]
    public string? DataControllerName { get; set; }

    [CliOption("--dns-domain-name")]
    public string? DnsDomainName { get; set; }

    [CliOption("--dns-replicas")]
    public string? DnsReplicas { get; set; }

    [CliOption("--domain-service-account-secret")]
    public int? DomainServiceAccountSecret { get; set; }

    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliOption("--netbios-domain-name")]
    public string? NetbiosDomainName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--ou-distinguished-name")]
    public string? OuDistinguishedName { get; set; }

    [CliFlag("--prefer-k8s-dns")]
    public bool? PreferK8sDns { get; set; }

    [CliOption("--primary-ad-dc-hostname")]
    public string? PrimaryAdDcHostname { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secondary-ad-dc-hostnames")]
    public string? SecondaryAdDcHostnames { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}