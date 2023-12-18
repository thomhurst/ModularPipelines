using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "provider-registration", "create")]
public record AzProviderhubProviderRegistrationCreateOptions(
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions
{
    [CommandSwitch("--capabilities")]
    public string? Capabilities { get; set; }

    [CommandSwitch("--incident-contact-email")]
    public string? IncidentContactEmail { get; set; }

    [CommandSwitch("--incident-routing-service")]
    public string? IncidentRoutingService { get; set; }

    [CommandSwitch("--incident-routing-team")]
    public string? IncidentRoutingTeam { get; set; }

    [CommandSwitch("--lighthouse-auth")]
    public string? LighthouseAuth { get; set; }

    [CommandSwitch("--managed-by-tenant-id")]
    public string? ManagedByTenantId { get; set; }

    [CommandSwitch("--manifest-owners")]
    public string? ManifestOwners { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--metadata-authn")]
    public string? MetadataAuthn { get; set; }

    [CommandSwitch("--metadata-authz")]
    public string? MetadataAuthz { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--opt-in-headers")]
    public string? OptInHeaders { get; set; }

    [CommandSwitch("--override-actions")]
    public string? OverrideActions { get; set; }

    [CommandSwitch("--provider-authentication")]
    public string? ProviderAuthentication { get; set; }

    [CommandSwitch("--provider-authorizations")]
    public string? ProviderAuthorizations { get; set; }

    [CommandSwitch("--provider-type")]
    public string? ProviderType { get; set; }

    [CommandSwitch("--provider-version")]
    public string? ProviderVersion { get; set; }

    [CommandSwitch("--req-features-policy")]
    public string? ReqFeaturesPolicy { get; set; }

    [CommandSwitch("--required-features")]
    public string? RequiredFeatures { get; set; }

    [CommandSwitch("--resource-access-policy")]
    public string? ResourceAccessPolicy { get; set; }

    [CommandSwitch("--resource-access-roles")]
    public string? ResourceAccessRoles { get; set; }

    [CommandSwitch("--schema-owners")]
    public string? SchemaOwners { get; set; }

    [CommandSwitch("--service-tree-infos")]
    public string? ServiceTreeInfos { get; set; }

    [CommandSwitch("--soft-delete-ttl")]
    public string? SoftDeleteTtl { get; set; }

    [CommandSwitch("--template-deployment-options")]
    public string? TemplateDeploymentOptions { get; set; }
}