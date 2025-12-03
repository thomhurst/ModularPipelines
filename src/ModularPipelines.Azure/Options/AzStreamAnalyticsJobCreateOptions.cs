using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stream-analytics", "job", "create")]
public record AzStreamAnalyticsJobCreateOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--arrival-max-delay")]
    public string? ArrivalMaxDelay { get; set; }

    [CliOption("--compatibility-level")]
    public string? CompatibilityLevel { get; set; }

    [CliOption("--content-storage-policy")]
    public string? ContentStoragePolicy { get; set; }

    [CliOption("--data-locale")]
    public string? DataLocale { get; set; }

    [CliOption("--functions")]
    public string? Functions { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--inputs")]
    public string? Inputs { get; set; }

    [CliOption("--job-storage-account")]
    public int? JobStorageAccount { get; set; }

    [CliOption("--job-type")]
    public string? JobType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--order-max-delay")]
    public string? OrderMaxDelay { get; set; }

    [CliOption("--out-of-order-policy")]
    public string? OutOfOrderPolicy { get; set; }

    [CliOption("--output-error-policy")]
    public string? OutputErrorPolicy { get; set; }

    [CliOption("--output-start-mode")]
    public string? OutputStartMode { get; set; }

    [CliOption("--output-start-time")]
    public string? OutputStartTime { get; set; }

    [CliOption("--outputs")]
    public string? Outputs { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--transformation")]
    public string? Transformation { get; set; }
}