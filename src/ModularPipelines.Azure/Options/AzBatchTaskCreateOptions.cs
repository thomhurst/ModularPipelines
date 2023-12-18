using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "task", "create")]
public record AzBatchTaskCreateOptions(
[property: CommandSwitch("--job-id")] string JobId
) : AzOptions
{
    [CommandSwitch("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--affinity-id")]
    public string? AffinityId { get; set; }

    [CommandSwitch("--application-package-references")]
    public string? ApplicationPackageReferences { get; set; }

    [CommandSwitch("--command-line")]
    public string? CommandLine { get; set; }

    [CommandSwitch("--environment-settings")]
    public string? EnvironmentSettings { get; set; }

    [CommandSwitch("--json-file")]
    public string? JsonFile { get; set; }

    [CommandSwitch("--max-task-retry-count")]
    public int? MaxTaskRetryCount { get; set; }

    [CommandSwitch("--max-wall-clock-time")]
    public string? MaxWallClockTime { get; set; }

    [CommandSwitch("--resource-files")]
    public string? ResourceFiles { get; set; }

    [CommandSwitch("--retention-time")]
    public string? RetentionTime { get; set; }

    [CommandSwitch("--task-id")]
    public string? TaskId { get; set; }
}

