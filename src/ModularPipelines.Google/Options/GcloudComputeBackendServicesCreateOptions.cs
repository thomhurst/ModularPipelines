using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "backend-services", "create")]
public record GcloudComputeBackendServicesCreateOptions(
[property: PositionalArgument] string BackendServiceName
) : GcloudOptions
{
    [CommandSwitch("--affinity-cookie-ttl")]
    public string? AffinityCookieTtl { get; set; }

    [CommandSwitch("--bypass-cache-on-request-headers")]
    public string? BypassCacheOnRequestHeaders { get; set; }

    [BooleanCommandSwitch("--cache-key-include-host")]
    public bool? CacheKeyIncludeHost { get; set; }

    [CommandSwitch("--cache-key-include-http-header")]
    public string[]? CacheKeyIncludeHttpHeader { get; set; }

    [CommandSwitch("--cache-key-include-named-cookie")]
    public string[]? CacheKeyIncludeNamedCookie { get; set; }

    [BooleanCommandSwitch("--cache-key-include-protocol")]
    public bool? CacheKeyIncludeProtocol { get; set; }

    [BooleanCommandSwitch("--cache-key-include-query-string")]
    public bool? CacheKeyIncludeQueryString { get; set; }

    [CommandSwitch("--cache-mode")]
    public string? CacheMode { get; set; }

    [CommandSwitch("--client-ttl")]
    public string? ClientTtl { get; set; }

    [CommandSwitch("--compression-mode")]
    public string? CompressionMode { get; set; }

    [BooleanCommandSwitch("--connection-drain-on-failover")]
    public bool? ConnectionDrainOnFailover { get; set; }

    [CommandSwitch("--connection-draining-timeout")]
    public string? ConnectionDrainingTimeout { get; set; }

    [CommandSwitch("--connection-persistence-on-unhealthy-backends")]
    public string? ConnectionPersistenceOnUnhealthyBackends { get; set; }

    [CommandSwitch("--custom-request-header")]
    public string? CustomRequestHeader { get; set; }

    [CommandSwitch("--custom-response-header")]
    public string? CustomResponseHeader { get; set; }

    [CommandSwitch("--default-ttl")]
    public string? DefaultTtl { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--drop-traffic-if-unhealthy")]
    public bool? DropTrafficIfUnhealthy { get; set; }

    [CommandSwitch("--[no-]enable-cdn")]
    public string[]? NoEnableCdn { get; set; }

    [CommandSwitch("--[no-]enable-logging")]
    public string[]? NoEnableLogging { get; set; }

    [CommandSwitch("--[no-]enable-strong-affinity")]
    public string[]? NoEnableStrongAffinity { get; set; }

    [CommandSwitch("--failover-ratio")]
    public string? FailoverRatio { get; set; }

    [CommandSwitch("--health-checks")]
    public string[]? HealthChecks { get; set; }

    [CommandSwitch("--http-health-checks")]
    public string[]? HttpHealthChecks { get; set; }

    [CommandSwitch("--https-health-checks")]
    public string[]? HttpsHealthChecks { get; set; }

    [CommandSwitch("--iap")]
    public string[]? Iap { get; set; }

    [CommandSwitch("--idle-timeout-sec")]
    public string? IdleTimeoutSec { get; set; }

    [CommandSwitch("--load-balancing-scheme")]
    public string? LoadBalancingScheme { get; set; }

    [CommandSwitch("--locality-lb-policy")]
    public string? LocalityLbPolicy { get; set; }

    [CommandSwitch("--logging-optional")]
    public string? LoggingOptional { get; set; }

    [CommandSwitch("--logging-optional-fields")]
    public string[]? LoggingOptionalFields { get; set; }

    [CommandSwitch("--logging-sample-rate")]
    public string? LoggingSampleRate { get; set; }

    [CommandSwitch("--max-ttl")]
    public string? MaxTtl { get; set; }

    [CommandSwitch("--[no-]negative-caching")]
    public string[]? NoNegativeCaching { get; set; }

    [CommandSwitch("--negative-caching-policy")]
    public string[]? NegativeCachingPolicy { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--port-name")]
    public string? PortName { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--[no-]request-coalescing")]
    public string[]? NoRequestCoalescing { get; set; }

    [CommandSwitch("--serve-while-stale")]
    public string? ServeWhileStale { get; set; }

    [CommandSwitch("--service-bindings")]
    public string[]? ServiceBindings { get; set; }

    [CommandSwitch("--session-affinity")]
    public string? SessionAffinity { get; set; }

    [CommandSwitch("--signed-url-cache-max-age")]
    public string? SignedUrlCacheMaxAge { get; set; }

    [CommandSwitch("--subsetting-policy")]
    public string? SubsettingPolicy { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--tracking-mode")]
    public string? TrackingMode { get; set; }

    [CommandSwitch("--cache-key-query-string-blacklist")]
    public string[]? CacheKeyQueryStringBlacklist { get; set; }

    [CommandSwitch("--cache-key-query-string-whitelist")]
    public string[]? CacheKeyQueryStringWhitelist { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [BooleanCommandSwitch("--global-health-checks")]
    public bool? GlobalHealthChecks { get; set; }

    [CommandSwitch("--health-checks-region")]
    public string? HealthChecksRegion { get; set; }
}