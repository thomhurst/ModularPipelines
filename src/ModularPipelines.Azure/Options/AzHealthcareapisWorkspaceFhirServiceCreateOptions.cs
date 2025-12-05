using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("healthcareapis", "workspace", "fhir-service", "create")]
public record AzHealthcareapisWorkspaceFhirServiceCreateOptions(
[property: CliOption("--fhir-service-name")] string FhirServiceName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--access-policies")]
    public string? AccessPolicies { get; set; }

    [CliOption("--authentication-configuration")]
    public string? AuthenticationConfiguration { get; set; }

    [CliOption("--cors-configuration")]
    public string? CorsConfiguration { get; set; }

    [CliOption("--default")]
    public string? Default { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--export-configuration-storage-account-name")]
    public int? ExportConfigurationStorageAccountName { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--login-servers")]
    public string? LoginServers { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--oci-artifacts")]
    public string? OciArtifacts { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--resource-type-overrides")]
    public string? ResourceTypeOverrides { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user-assigned-identities")]
    public string? UserAssignedIdentities { get; set; }
}