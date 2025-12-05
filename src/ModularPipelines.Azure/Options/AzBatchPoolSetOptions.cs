using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "pool", "set")]
public record AzBatchPoolSetOptions(
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

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }

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

    [CliOption("--start-task-resource-files")]
    public string? StartTaskResourceFiles { get; set; }

    [CliOption("--start-task-wait-for-success")]
    public string? StartTaskWaitForSuccess { get; set; }

    [CliOption("--target-communication")]
    public string? TargetCommunication { get; set; }
}