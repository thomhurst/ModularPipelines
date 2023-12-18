using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "pool", "create", "(ure-batch-cli-extensions", "extension)")]
public record AzBatchPoolCreateAzureBatchCliExtensionsExtensionOptions(
[property: CommandSwitch("--pool-id")] string PoolId
) : AzOptions
{
    [CommandSwitch("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--application-licenses")]
    public string? ApplicationLicenses { get; set; }

    [CommandSwitch("--application-package-references")]
    public string? ApplicationPackageReferences { get; set; }

    [CommandSwitch("--auto-scale-formula")]
    public string? AutoScaleFormula { get; set; }

    [CommandSwitch("--certificate-references")]
    public string? CertificateReferences { get; set; }

    [CommandSwitch("--disk-encryption-targets")]
    public string? DiskEncryptionTargets { get; set; }

    [BooleanCommandSwitch("--enable-inter-node-communication")]
    public bool? EnableInterNodeCommunication { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--json-file")]
    public string? JsonFile { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--node-agent-sku-id")]
    public string? NodeAgentSkuId { get; set; }

    [CommandSwitch("--os-family")]
    public string? OsFamily { get; set; }

    [CommandSwitch("--os-version")]
    public string? OsVersion { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--resize-timeout")]
    public string? ResizeTimeout { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--start-task-command-line")]
    public string? StartTaskCommandLine { get; set; }

    [CommandSwitch("--start-task-resource-files")]
    public string? StartTaskResourceFiles { get; set; }

    [BooleanCommandSwitch("--start-task-wait-for-success")]
    public bool? StartTaskWaitForSuccess { get; set; }

    [CommandSwitch("--target-dedicated-nodes")]
    public string? TargetDedicatedNodes { get; set; }

    [CommandSwitch("--target-low-priority-nodes")]
    public string? TargetLowPriorityNodes { get; set; }

    [CommandSwitch("--targets")]
    public string? Targets { get; set; }

    [CommandSwitch("--task-slots-per-node")]
    public string? TaskSlotsPerNode { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [CommandSwitch("--vm-size")]
    public string? VmSize { get; set; }
}