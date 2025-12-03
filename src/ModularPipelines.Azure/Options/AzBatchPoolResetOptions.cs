using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "pool", "reset")]
public record AzBatchPoolResetOptions(
[property: CliOption("--pool-id")] string PoolId
) : AzOptions
{
    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--application-package-references")]
    public string? ApplicationPackageReferences { get; set; }

    [CliOption("--certificate-references")]
    public string? CertificateReferences { get; set; }

    [CliOption("--json-file")]
    public string? JsonFile { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--start-task-command-line")]
    public string? StartTaskCommandLine { get; set; }

    [CliOption("--start-task-environment-settings")]
    public string? StartTaskEnvironmentSettings { get; set; }

    [CliOption("--start-task-max-task-retry-count")]
    public int? StartTaskMaxTaskRetryCount { get; set; }

    [CliOption("--start-task-wait-for-success")]
    public string? StartTaskWaitForSuccess { get; set; }
}