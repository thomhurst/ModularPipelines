using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcareapis", "service", "create")]
public record AzHealthcareapisServiceCreateOptions(
[property: CommandSwitch("--kind")] string Kind,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName
) : AzOptions
{
    [CommandSwitch("--access-policies")]
    public string? AccessPolicies { get; set; }

    [CommandSwitch("--authentication-configuration")]
    public string? AuthenticationConfiguration { get; set; }

    [CommandSwitch("--cors-configuration")]
    public string? CorsConfiguration { get; set; }

    [CommandSwitch("--cosmos-db-configuration")]
    public string? CosmosDbConfiguration { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--export-configuration-storage-account-name")]
    public int? ExportConfigurationStorageAccountName { get; set; }

    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [CommandSwitch("--login-servers")]
    public string? LoginServers { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--oci-artifacts")]
    public string? OciArtifacts { get; set; }

    [CommandSwitch("--private-endpoint-connections")]
    public string? PrivateEndpointConnections { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}