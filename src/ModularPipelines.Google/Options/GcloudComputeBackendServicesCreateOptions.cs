using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-services", "create")]
public record GcloudComputeBackendServicesCreateOptions(
[property: CliArgument] string BackendServiceName
) : GcloudOptions
{
    [CliOption("--affinity-cookie-ttl")]
    public string? AffinityCookieTtl { get; set; }

    [CliOption("--bypass-cache-on-request-headers")]
    public string? BypassCacheOnRequestHeaders { get; set; }

    [CliFlag("--cache-key-include-host")]
    public bool? CacheKeyIncludeHost { get; set; }

    [CliOption("--cache-key-include-http-header")]
    public string[]? CacheKeyIncludeHttpHeader { get; set; }

    [CliOption("--cache-key-include-named-cookie")]
    public string[]? CacheKeyIncludeNamedCookie { get; set; }

    [CliFlag("--cache-key-include-protocol")]
    public bool? CacheKeyIncludeProtocol { get; set; }

    [CliFlag("--cache-key-include-query-string")]
    public bool? CacheKeyIncludeQueryString { get; set; }

    [CliOption("--cache-mode")]
    public string? CacheMode { get; set; }

    [CliOption("--client-ttl")]
    public string? ClientTtl { get; set; }

    [CliOption("--compression-mode")]
    public string? CompressionMode { get; set; }

    [CliFlag("--connection-drain-on-failover")]
    public bool? ConnectionDrainOnFailover { get; set; }

    [CliOption("--connection-draining-timeout")]
    public string? ConnectionDrainingTimeout { get; set; }

    [CliOption("--connection-persistence-on-unhealthy-backends")]
    public string? ConnectionPersistenceOnUnhealthyBackends { get; set; }

    [CliOption("--custom-request-header")]
    public string? CustomRequestHeader { get; set; }

    [CliOption("--custom-response-header")]
    public string? CustomResponseHeader { get; set; }

    [CliOption("--default-ttl")]
    public string? DefaultTtl { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--drop-traffic-if-unhealthy")]
    public bool? DropTrafficIfUnhealthy { get; set; }

    [CliOption("--[no-]enable-cdn")]
    public string[]? NoEnableCdn { get; set; }

    [CliOption("--[no-]enable-logging")]
    public string[]? NoEnableLogging { get; set; }

    [CliOption("--[no-]enable-strong-affinity")]
    public string[]? NoEnableStrongAffinity { get; set; }

    [CliOption("--failover-ratio")]
    public string? FailoverRatio { get; set; }

    [CliOption("--health-checks")]
    public string[]? HealthChecks { get; set; }

    [CliOption("--http-health-checks")]
    public string[]? HttpHealthChecks { get; set; }

    [CliOption("--https-health-checks")]
    public string[]? HttpsHealthChecks { get; set; }

    [CliOption("--iap")]
    public string[]? Iap { get; set; }

    [CliOption("--idle-timeout-sec")]
    public string? IdleTimeoutSec { get; set; }

    [CliOption("--load-balancing-scheme")]
    public string? LoadBalancingScheme { get; set; }

    [CliOption("--locality-lb-policy")]
    public string? LocalityLbPolicy { get; set; }

    [CliOption("--logging-optional")]
    public string? LoggingOptional { get; set; }

    [CliOption("--logging-optional-fields")]
    public string[]? LoggingOptionalFields { get; set; }

    [CliOption("--logging-sample-rate")]
    public string? LoggingSampleRate { get; set; }

    [CliOption("--max-ttl")]
    public string? MaxTtl { get; set; }

    [CliOption("--[no-]negative-caching")]
    public string[]? NoNegativeCaching { get; set; }

    [CliOption("--negative-caching-policy")]
    public string[]? NegativeCachingPolicy { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--port-name")]
    public string? PortName { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--[no-]request-coalescing")]
    public string[]? NoRequestCoalescing { get; set; }

    [CliOption("--serve-while-stale")]
    public string? ServeWhileStale { get; set; }

    [CliOption("--service-bindings")]
    public string[]? ServiceBindings { get; set; }

    [CliOption("--session-affinity")]
    public string? SessionAffinity { get; set; }

    [CliOption("--signed-url-cache-max-age")]
    public string? SignedUrlCacheMaxAge { get; set; }

    [CliOption("--subsetting-policy")]
    public string? SubsettingPolicy { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--tracking-mode")]
    public string? TrackingMode { get; set; }

    [CliOption("--cache-key-query-string-blacklist")]
    public string[]? CacheKeyQueryStringBlacklist { get; set; }

    [CliOption("--cache-key-query-string-whitelist")]
    public string[]? CacheKeyQueryStringWhitelist { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliFlag("--global-health-checks")]
    public bool? GlobalHealthChecks { get; set; }

    [CliOption("--health-checks-region")]
    public string? HealthChecksRegion { get; set; }
}