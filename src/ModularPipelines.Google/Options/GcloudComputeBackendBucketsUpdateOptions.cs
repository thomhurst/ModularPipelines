using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "backend-buckets", "update")]
public record GcloudComputeBackendBucketsUpdateOptions(
[property: PositionalArgument] string BackendBucketName
) : GcloudOptions
{
    [CommandSwitch("--cache-key-include-http-header")]
    public string[]? CacheKeyIncludeHttpHeader { get; set; }

    [CommandSwitch("--cache-key-query-string-whitelist")]
    public string[]? CacheKeyQueryStringWhitelist { get; set; }

    [CommandSwitch("--cache-mode")]
    public string? CacheMode { get; set; }

    [CommandSwitch("--compression-mode")]
    public string? CompressionMode { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--edge-security-policy")]
    public string? EdgeSecurityPolicy { get; set; }

    [CommandSwitch("--[no-]enable-cdn")]
    public string[]? NoEnableCdn { get; set; }

    [CommandSwitch("--gcs-bucket-name")]
    public string? GcsBucketName { get; set; }

    [CommandSwitch("--[no-]request-coalescing")]
    public string[]? NoRequestCoalescing { get; set; }

    [CommandSwitch("--signed-url-cache-max-age")]
    public string? SignedUrlCacheMaxAge { get; set; }

    [CommandSwitch("--bypass-cache-on-request-headers")]
    public string? BypassCacheOnRequestHeaders { get; set; }

    [BooleanCommandSwitch("--no-bypass-cache-on-request-headers")]
    public bool? NoBypassCacheOnRequestHeaders { get; set; }

    [CommandSwitch("--client-ttl")]
    public string? ClientTtl { get; set; }

    [BooleanCommandSwitch("--no-client-ttl")]
    public bool? NoClientTtl { get; set; }

    [CommandSwitch("--custom-response-header")]
    public string? CustomResponseHeader { get; set; }

    [BooleanCommandSwitch("--no-custom-response-headers")]
    public bool? NoCustomResponseHeaders { get; set; }

    [CommandSwitch("--default-ttl")]
    public string? DefaultTtl { get; set; }

    [BooleanCommandSwitch("--no-default-ttl")]
    public bool? NoDefaultTtl { get; set; }

    [CommandSwitch("--max-ttl")]
    public string? MaxTtl { get; set; }

    [BooleanCommandSwitch("--no-max-ttl")]
    public bool? NoMaxTtl { get; set; }

    [CommandSwitch("--[no-]negative-caching")]
    public string[]? NoNegativeCaching { get; set; }

    [BooleanCommandSwitch("--no-negative-caching-policies")]
    public bool? NoNegativeCachingPolicies { get; set; }

    [CommandSwitch("--negative-caching-policy")]
    public string[]? NegativeCachingPolicy { get; set; }

    [CommandSwitch("--serve-while-stale")]
    public string? ServeWhileStale { get; set; }

    [BooleanCommandSwitch("--no-serve-while-stale")]
    public bool? NoServeWhileStale { get; set; }
}