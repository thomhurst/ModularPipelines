using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workbench", "instances", "update")]
public record GcloudWorkbenchInstancesUpdateOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--accelerator-core-count")]
    public string? AcceleratorCoreCount { get; set; }

    [CommandSwitch("--accelerator-type")]
    public string? AcceleratorType { get; set; }

    [CommandSwitch("--custom-gpu-driver-path")]
    public string? CustomGpuDriverPath { get; set; }

    [CommandSwitch("--install-gpu-driver")]
    public string? InstallGpuDriver { get; set; }

    [CommandSwitch("--shielded-integrity-monitoring")]
    public string? ShieldedIntegrityMonitoring { get; set; }

    [CommandSwitch("--shielded-secure-boot")]
    public string? ShieldedSecureBoot { get; set; }

    [CommandSwitch("--shielded-vtpm")]
    public string? ShieldedVtpm { get; set; }
}