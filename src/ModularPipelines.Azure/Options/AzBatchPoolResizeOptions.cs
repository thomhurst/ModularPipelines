using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "pool", "resize")]
public record AzBatchPoolResizeOptions(
[property: CliOption("--pool-id")] string PoolId
) : AzOptions
{
    [CliFlag("--abort")]
    public bool? Abort { get; set; }

    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }

    [CliOption("--node-deallocation-option")]
    public string? NodeDeallocationOption { get; set; }

    [CliOption("--resize-timeout")]
    public string? ResizeTimeout { get; set; }

    [CliOption("--target-dedicated-nodes")]
    public string? TargetDedicatedNodes { get; set; }

    [CliOption("--target-low-priority-nodes")]
    public string? TargetLowPriorityNodes { get; set; }
}