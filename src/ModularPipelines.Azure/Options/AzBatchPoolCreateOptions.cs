using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "pool", "create")]
public record AzBatchPoolCreateOptions : AzOptions
{
    [CliFlag("--accelerated-networking")]
    public bool? AcceleratedNetworking { get; set; }

    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--application-licenses")]
    public string? ApplicationLicenses { get; set; }

    [CliOption("--application-package-references")]
    public string? ApplicationPackageReferences { get; set; }

    [CliOption("--auto-scale-formula")]
    public string? AutoScaleFormula { get; set; }

    [CliOption("--certificate-references")]
    public string? CertificateReferences { get; set; }

    [CliOption("--disk-encryption-targets")]
    public string? DiskEncryptionTargets { get; set; }

    [CliFlag("--enable-inter-node-communication")]
    public bool? EnableInterNodeCommunication { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--json-file")]
    public string? JsonFile { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--node-agent-sku-id")]
    public string? NodeAgentSkuId { get; set; }

    [CliOption("--os-family")]
    public string? OsFamily { get; set; }

    [CliOption("--os-version")]
    public string? OsVersion { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--resize-timeout")]
    public string? ResizeTimeout { get; set; }

    [CliOption("--start-task-command-line")]
    public string? StartTaskCommandLine { get; set; }

    [CliOption("--start-task-resource-files")]
    public string? StartTaskResourceFiles { get; set; }

    [CliOption("--start-task-wait-for-success")]
    public string? StartTaskWaitForSuccess { get; set; }

    [CliOption("--target-communication")]
    public string? TargetCommunication { get; set; }

    [CliOption("--target-dedicated-nodes")]
    public string? TargetDedicatedNodes { get; set; }

    [CliOption("--target-low-priority-nodes")]
    public string? TargetLowPriorityNodes { get; set; }

    [CliOption("--targets")]
    public string? Targets { get; set; }

    [CliOption("--task-slots-per-node")]
    public string? TaskSlotsPerNode { get; set; }

    [CliOption("--vm-size")]
    public string? VmSize { get; set; }
}