using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "resource-type-registration", "create")]
public record AzProviderhubResourceTypeRegistrationCreateOptions(
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AzOptions
{
    [BooleanCommandSwitch("--allowed-unauthorized-actions")]
    public bool? AllowedUnauthorizedActions { get; set; }

    [CommandSwitch("--auth-mappings")]
    public string? AuthMappings { get; set; }

    [CommandSwitch("--check-name-availability-specifications")]
    public string? CheckNameAvailabilitySpecifications { get; set; }

    [CommandSwitch("--creation-begin")]
    public string? CreationBegin { get; set; }

    [CommandSwitch("--dav")]
    public string? Dav { get; set; }

    [CommandSwitch("--default-api-version")]
    public string? DefaultApiVersion { get; set; }

    [CommandSwitch("--deletion-policy")]
    public string? DeletionPolicy { get; set; }

    [BooleanCommandSwitch("--enable-async-operation")]
    public bool? EnableAsyncOperation { get; set; }

    [BooleanCommandSwitch("--enable-third-party-s2s")]
    public bool? EnableThirdPartyS2s { get; set; }

    [CommandSwitch("--endpoints")]
    public string? Endpoints { get; set; }

    [CommandSwitch("--extended-locations")]
    public string? ExtendedLocations { get; set; }

    [CommandSwitch("--identity-management")]
    public string? IdentityManagement { get; set; }

    [BooleanCommandSwitch("--is-pure-proxy")]
    public bool? IsPureProxy { get; set; }

    [CommandSwitch("--linked-access-checks")]
    public string? LinkedAccessChecks { get; set; }

    [CommandSwitch("--logging-rules")]
    public string? LoggingRules { get; set; }

    [CommandSwitch("--marketplace-type")]
    public string? MarketplaceType { get; set; }

    [CommandSwitch("--opt-in-headers")]
    public string? OptInHeaders { get; set; }

    [CommandSwitch("--override-actions")]
    public string? OverrideActions { get; set; }

    [CommandSwitch("--patch-begin")]
    public string? PatchBegin { get; set; }

    [CommandSwitch("--regionality")]
    public string? Regionality { get; set; }

    [CommandSwitch("--req-features-policy")]
    public string? ReqFeaturesPolicy { get; set; }

    [CommandSwitch("--required-features")]
    public string? RequiredFeatures { get; set; }

    [CommandSwitch("--resource-move-policy")]
    public string? ResourceMovePolicy { get; set; }

    [CommandSwitch("--routing-type")]
    public string? RoutingType { get; set; }

    [CommandSwitch("--service-tree-infos")]
    public string? ServiceTreeInfos { get; set; }

    [CommandSwitch("--soft-delete-ttl")]
    public string? SoftDeleteTtl { get; set; }

    [CommandSwitch("--sub-state-rules")]
    public string? SubStateRules { get; set; }

    [CommandSwitch("--swagger-specifications")]
    public string? SwaggerSpecifications { get; set; }

    [CommandSwitch("--template-deployment-options")]
    public string? TemplateDeploymentOptions { get; set; }

    [CommandSwitch("--throttling-rules")]
    public string? ThrottlingRules { get; set; }
}