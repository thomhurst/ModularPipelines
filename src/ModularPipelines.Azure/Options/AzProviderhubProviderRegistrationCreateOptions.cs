using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("providerhub", "provider-registration", "create")]
public record AzProviderhubProviderRegistrationCreateOptions(
[property: CliOption("--provider-namespace")] string ProviderNamespace
) : AzOptions
{
    [CliOption("--capabilities")]
    public string? Capabilities { get; set; }

    [CliOption("--incident-contact-email")]
    public string? IncidentContactEmail { get; set; }

    [CliOption("--incident-routing-service")]
    public string? IncidentRoutingService { get; set; }

    [CliOption("--incident-routing-team")]
    public string? IncidentRoutingTeam { get; set; }

    [CliOption("--lighthouse-auth")]
    public string? LighthouseAuth { get; set; }

    [CliOption("--managed-by-tenant-id")]
    public string? ManagedByTenantId { get; set; }

    [CliOption("--manifest-owners")]
    public string? ManifestOwners { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--metadata-authn")]
    public string? MetadataAuthn { get; set; }

    [CliOption("--metadata-authz")]
    public string? MetadataAuthz { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--opt-in-headers")]
    public string? OptInHeaders { get; set; }

    [CliOption("--override-actions")]
    public string? OverrideActions { get; set; }

    [CliOption("--provider-authentication")]
    public string? ProviderAuthentication { get; set; }

    [CliOption("--provider-authorizations")]
    public string? ProviderAuthorizations { get; set; }

    [CliOption("--provider-type")]
    public string? ProviderType { get; set; }

    [CliOption("--provider-version")]
    public string? ProviderVersion { get; set; }

    [CliOption("--req-features-policy")]
    public string? ReqFeaturesPolicy { get; set; }

    [CliOption("--required-features")]
    public string? RequiredFeatures { get; set; }

    [CliOption("--resource-access-policy")]
    public string? ResourceAccessPolicy { get; set; }

    [CliOption("--resource-access-roles")]
    public string? ResourceAccessRoles { get; set; }

    [CliOption("--schema-owners")]
    public string? SchemaOwners { get; set; }

    [CliOption("--service-tree-infos")]
    public string? ServiceTreeInfos { get; set; }

    [CliOption("--soft-delete-ttl")]
    public string? SoftDeleteTtl { get; set; }

    [CliOption("--template-deployment-options")]
    public string? TemplateDeploymentOptions { get; set; }
}