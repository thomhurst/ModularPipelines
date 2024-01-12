using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "backend-buckets", "create")]
public record GcloudComputeBackendBucketsCreateOptions(
[property: PositionalArgument] string BackendBucketName,
[property: CommandSwitch("--gcs-bucket-name")] string GcsBucketName
) : GcloudOptions
{
    [CommandSwitch("--bypass-cache-on-request-headers")]
    public string? BypassCacheOnRequestHeaders { get; set; }

    [CommandSwitch("--cache-key-include-http-header")]
    public string[]? CacheKeyIncludeHttpHeader { get; set; }

    [CommandSwitch("--cache-key-query-string-whitelist")]
    public string[]? CacheKeyQueryStringWhitelist { get; set; }

    [CommandSwitch("--cache-mode")]
    public string? CacheMode { get; set; }

    [CommandSwitch("--client-ttl")]
    public string? ClientTtl { get; set; }

    [CommandSwitch("--compression-mode")]
    public string? CompressionMode { get; set; }

    [CommandSwitch("--custom-response-header")]
    public string? CustomResponseHeader { get; set; }

    [CommandSwitch("--default-ttl")]
    public string? DefaultTtl { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--[no-]enable-cdn")]
    public string[]? NoEnableCdn { get; set; }

    [CommandSwitch("--max-ttl")]
    public string? MaxTtl { get; set; }

    [CommandSwitch("--[no-]negative-caching")]
    public string[]? NoNegativeCaching { get; set; }

    [CommandSwitch("--negative-caching-policy")]
    public string[]? NegativeCachingPolicy { get; set; }

    [CommandSwitch("--[no-]request-coalescing")]
    public string[]? NoRequestCoalescing { get; set; }

    [CommandSwitch("--serve-while-stale")]
    public string? ServeWhileStale { get; set; }

    [CommandSwitch("--signed-url-cache-max-age")]
    public string? SignedUrlCacheMaxAge { get; set; }
}