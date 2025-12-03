using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcareapis", "service", "create")]
public record AzHealthcareapisServiceCreateOptions(
[property: CliOption("--kind")] string Kind,
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName
) : AzOptions
{
    [CliOption("--access-policies")]
    public string? AccessPolicies { get; set; }

    [CliOption("--authentication-configuration")]
    public string? AuthenticationConfiguration { get; set; }

    [CliOption("--cors-configuration")]
    public string? CorsConfiguration { get; set; }

    [CliOption("--cosmos-db-configuration")]
    public string? CosmosDbConfiguration { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--export-configuration-storage-account-name")]
    public int? ExportConfigurationStorageAccountName { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--login-servers")]
    public string? LoginServers { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--oci-artifacts")]
    public string? OciArtifacts { get; set; }

    [CliOption("--private-endpoint-connections")]
    public string? PrivateEndpointConnections { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}