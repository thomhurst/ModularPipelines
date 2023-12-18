using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcareapis", "workspace", "fhir-service", "create")]
public record AzHealthcareapisWorkspaceFhirServiceCreateOptions(
[property: CommandSwitch("--fhir-service-name")] string FhirServiceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--access-policies")]
    public string? AccessPolicies { get; set; }

    [CommandSwitch("--authentication-configuration")]
    public string? AuthenticationConfiguration { get; set; }

    [CommandSwitch("--cors-configuration")]
    public string? CorsConfiguration { get; set; }

    [CommandSwitch("--default")]
    public string? Default { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--export-configuration-storage-account-name")]
    public int? ExportConfigurationStorageAccountName { get; set; }

    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--login-servers")]
    public string? LoginServers { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--oci-artifacts")]
    public string? OciArtifacts { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--resource-type-overrides")]
    public string? ResourceTypeOverrides { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--user-assigned-identities")]
    public string? UserAssignedIdentities { get; set; }
}

