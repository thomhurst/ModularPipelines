using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-buckets", "create")]
public record GcloudComputeBackendBucketsCreateOptions(
[property: CliArgument] string BackendBucketName,
[property: CliOption("--gcs-bucket-name")] string GcsBucketName
) : GcloudOptions
{
    [CliOption("--bypass-cache-on-request-headers")]
    public string? BypassCacheOnRequestHeaders { get; set; }

    [CliOption("--cache-key-include-http-header")]
    public string[]? CacheKeyIncludeHttpHeader { get; set; }

    [CliOption("--cache-key-query-string-whitelist")]
    public string[]? CacheKeyQueryStringWhitelist { get; set; }

    [CliOption("--cache-mode")]
    public string? CacheMode { get; set; }

    [CliOption("--client-ttl")]
    public string? ClientTtl { get; set; }

    [CliOption("--compression-mode")]
    public string? CompressionMode { get; set; }

    [CliOption("--custom-response-header")]
    public string? CustomResponseHeader { get; set; }

    [CliOption("--default-ttl")]
    public string? DefaultTtl { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--[no-]enable-cdn")]
    public string[]? NoEnableCdn { get; set; }

    [CliOption("--max-ttl")]
    public string? MaxTtl { get; set; }

    [CliOption("--[no-]negative-caching")]
    public string[]? NoNegativeCaching { get; set; }

    [CliOption("--negative-caching-policy")]
    public string[]? NegativeCachingPolicy { get; set; }

    [CliOption("--[no-]request-coalescing")]
    public string[]? NoRequestCoalescing { get; set; }

    [CliOption("--serve-while-stale")]
    public string? ServeWhileStale { get; set; }

    [CliOption("--signed-url-cache-max-age")]
    public string? SignedUrlCacheMaxAge { get; set; }
}