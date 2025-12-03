using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("providerhub", "resource-type-registration", "create")]
public record AzProviderhubResourceTypeRegistrationCreateOptions(
[property: CliOption("--provider-namespace")] string ProviderNamespace,
[property: CliOption("--resource-type")] string ResourceType
) : AzOptions
{
    [CliFlag("--allowed-unauthorized-actions")]
    public bool? AllowedUnauthorizedActions { get; set; }

    [CliOption("--auth-mappings")]
    public string? AuthMappings { get; set; }

    [CliOption("--check-name-availability-specifications")]
    public string? CheckNameAvailabilitySpecifications { get; set; }

    [CliOption("--creation-begin")]
    public string? CreationBegin { get; set; }

    [CliOption("--dav")]
    public string? Dav { get; set; }

    [CliOption("--default-api-version")]
    public string? DefaultApiVersion { get; set; }

    [CliOption("--deletion-policy")]
    public string? DeletionPolicy { get; set; }

    [CliFlag("--enable-async-operation")]
    public bool? EnableAsyncOperation { get; set; }

    [CliFlag("--enable-third-party-s2s")]
    public bool? EnableThirdPartyS2s { get; set; }

    [CliOption("--endpoints")]
    public string? Endpoints { get; set; }

    [CliOption("--extended-locations")]
    public string? ExtendedLocations { get; set; }

    [CliOption("--identity-management")]
    public string? IdentityManagement { get; set; }

    [CliFlag("--is-pure-proxy")]
    public bool? IsPureProxy { get; set; }

    [CliOption("--linked-access-checks")]
    public string? LinkedAccessChecks { get; set; }

    [CliOption("--logging-rules")]
    public string? LoggingRules { get; set; }

    [CliOption("--marketplace-type")]
    public string? MarketplaceType { get; set; }

    [CliOption("--opt-in-headers")]
    public string? OptInHeaders { get; set; }

    [CliOption("--override-actions")]
    public string? OverrideActions { get; set; }

    [CliOption("--patch-begin")]
    public string? PatchBegin { get; set; }

    [CliOption("--regionality")]
    public string? Regionality { get; set; }

    [CliOption("--req-features-policy")]
    public string? ReqFeaturesPolicy { get; set; }

    [CliOption("--required-features")]
    public string? RequiredFeatures { get; set; }

    [CliOption("--resource-move-policy")]
    public string? ResourceMovePolicy { get; set; }

    [CliOption("--routing-type")]
    public string? RoutingType { get; set; }

    [CliOption("--service-tree-infos")]
    public string? ServiceTreeInfos { get; set; }

    [CliOption("--soft-delete-ttl")]
    public string? SoftDeleteTtl { get; set; }

    [CliOption("--sub-state-rules")]
    public string? SubStateRules { get; set; }

    [CliOption("--swagger-specifications")]
    public string? SwaggerSpecifications { get; set; }

    [CliOption("--template-deployment-options")]
    public string? TemplateDeploymentOptions { get; set; }

    [CliOption("--throttling-rules")]
    public string? ThrottlingRules { get; set; }
}