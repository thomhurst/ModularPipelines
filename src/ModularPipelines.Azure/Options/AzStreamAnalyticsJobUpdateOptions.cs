using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stream-analytics", "job", "update")]
public record AzStreamAnalyticsJobUpdateOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--arrival-max-delay")]
    public string? ArrivalMaxDelay { get; set; }

    [CommandSwitch("--compatibility-level")]
    public string? CompatibilityLevel { get; set; }

    [CommandSwitch("--content-storage-policy")]
    public string? ContentStoragePolicy { get; set; }

    [CommandSwitch("--data-locale")]
    public string? DataLocale { get; set; }

    [CommandSwitch("--functions")]
    public string? Functions { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--inputs")]
    public string? Inputs { get; set; }

    [CommandSwitch("--job-storage-account")]
    public int? JobStorageAccount { get; set; }

    [CommandSwitch("--job-type")]
    public string? JobType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--order-max-delay")]
    public string? OrderMaxDelay { get; set; }

    [CommandSwitch("--out-of-order-policy")]
    public string? OutOfOrderPolicy { get; set; }

    [CommandSwitch("--output-error-policy")]
    public string? OutputErrorPolicy { get; set; }

    [CommandSwitch("--output-start-mode")]
    public string? OutputStartMode { get; set; }

    [CommandSwitch("--output-start-time")]
    public string? OutputStartTime { get; set; }

    [CommandSwitch("--outputs")]
    public string? Outputs { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--transformation")]
    public string? Transformation { get; set; }
}

