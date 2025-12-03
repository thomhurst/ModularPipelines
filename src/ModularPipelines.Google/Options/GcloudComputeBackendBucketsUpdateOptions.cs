using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-buckets", "update")]
public record GcloudComputeBackendBucketsUpdateOptions(
[property: CliArgument] string BackendBucketName
) : GcloudOptions
{
    [CliOption("--cache-key-include-http-header")]
    public string[]? CacheKeyIncludeHttpHeader { get; set; }

    [CliOption("--cache-key-query-string-whitelist")]
    public string[]? CacheKeyQueryStringWhitelist { get; set; }

    [CliOption("--cache-mode")]
    public string? CacheMode { get; set; }

    [CliOption("--compression-mode")]
    public string? CompressionMode { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--edge-security-policy")]
    public string? EdgeSecurityPolicy { get; set; }

    [CliOption("--[no-]enable-cdn")]
    public string[]? NoEnableCdn { get; set; }

    [CliOption("--gcs-bucket-name")]
    public string? GcsBucketName { get; set; }

    [CliOption("--[no-]request-coalescing")]
    public string[]? NoRequestCoalescing { get; set; }

    [CliOption("--signed-url-cache-max-age")]
    public string? SignedUrlCacheMaxAge { get; set; }

    [CliOption("--bypass-cache-on-request-headers")]
    public string? BypassCacheOnRequestHeaders { get; set; }

    [CliFlag("--no-bypass-cache-on-request-headers")]
    public bool? NoBypassCacheOnRequestHeaders { get; set; }

    [CliOption("--client-ttl")]
    public string? ClientTtl { get; set; }

    [CliFlag("--no-client-ttl")]
    public bool? NoClientTtl { get; set; }

    [CliOption("--custom-response-header")]
    public string? CustomResponseHeader { get; set; }

    [CliFlag("--no-custom-response-headers")]
    public bool? NoCustomResponseHeaders { get; set; }

    [CliOption("--default-ttl")]
    public string? DefaultTtl { get; set; }

    [CliFlag("--no-default-ttl")]
    public bool? NoDefaultTtl { get; set; }

    [CliOption("--max-ttl")]
    public string? MaxTtl { get; set; }

    [CliFlag("--no-max-ttl")]
    public bool? NoMaxTtl { get; set; }

    [CliOption("--[no-]negative-caching")]
    public string[]? NoNegativeCaching { get; set; }

    [CliFlag("--no-negative-caching-policies")]
    public bool? NoNegativeCachingPolicies { get; set; }

    [CliOption("--negative-caching-policy")]
    public string[]? NegativeCachingPolicy { get; set; }

    [CliOption("--serve-while-stale")]
    public string? ServeWhileStale { get; set; }

    [CliFlag("--no-serve-while-stale")]
    public bool? NoServeWhileStale { get; set; }
}