using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "pool", "reset")]
public record AzBatchPoolResetOptions(
[property: CommandSwitch("--pool-id")] string PoolId
) : AzOptions
{
    [CommandSwitch("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--application-package-references")]
    public string? ApplicationPackageReferences { get; set; }

    [CommandSwitch("--certificate-references")]
    public string? CertificateReferences { get; set; }

    [CommandSwitch("--json-file")]
    public string? JsonFile { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--start-task-command-line")]
    public string? StartTaskCommandLine { get; set; }

    [CommandSwitch("--start-task-environment-settings")]
    public string? StartTaskEnvironmentSettings { get; set; }

    [CommandSwitch("--start-task-max-task-retry-count")]
    public int? StartTaskMaxTaskRetryCount { get; set; }

    [CommandSwitch("--start-task-wait-for-success")]
    public string? StartTaskWaitForSuccess { get; set; }
}

