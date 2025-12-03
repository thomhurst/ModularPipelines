using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "task", "create")]
public record AzBatchTaskCreateOptions(
[property: CliOption("--job-id")] string JobId
) : AzOptions
{
    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--affinity-id")]
    public string? AffinityId { get; set; }

    [CliOption("--application-package-references")]
    public string? ApplicationPackageReferences { get; set; }

    [CliOption("--command-line")]
    public string? CommandLine { get; set; }

    [CliOption("--environment-settings")]
    public string? EnvironmentSettings { get; set; }

    [CliOption("--json-file")]
    public string? JsonFile { get; set; }

    [CliOption("--max-task-retry-count")]
    public int? MaxTaskRetryCount { get; set; }

    [CliOption("--max-wall-clock-time")]
    public string? MaxWallClockTime { get; set; }

    [CliOption("--resource-files")]
    public string? ResourceFiles { get; set; }

    [CliOption("--retention-time")]
    public string? RetentionTime { get; set; }

    [CliOption("--task-id")]
    public string? TaskId { get; set; }
}