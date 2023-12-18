using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "pool", "resize")]
public record AzBatchPoolResizeOptions(
[property: CommandSwitch("--pool-id")] string PoolId
) : AzOptions
{
    [BooleanCommandSwitch("--abort")]
    public bool? Abort { get; set; }

    [CommandSwitch("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CommandSwitch("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }

    [CommandSwitch("--node-deallocation-option")]
    public string? NodeDeallocationOption { get; set; }

    [CommandSwitch("--resize-timeout")]
    public string? ResizeTimeout { get; set; }

    [CommandSwitch("--target-dedicated-nodes")]
    public string? TargetDedicatedNodes { get; set; }

    [CommandSwitch("--target-low-priority-nodes")]
    public string? TargetLowPriorityNodes { get; set; }
}