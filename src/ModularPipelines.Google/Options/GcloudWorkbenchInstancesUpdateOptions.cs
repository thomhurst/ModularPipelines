using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workbench", "instances", "update")]
public record GcloudWorkbenchInstancesUpdateOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--accelerator-core-count")]
    public string? AcceleratorCoreCount { get; set; }

    [CliOption("--accelerator-type")]
    public string? AcceleratorType { get; set; }

    [CliOption("--custom-gpu-driver-path")]
    public string? CustomGpuDriverPath { get; set; }

    [CliOption("--install-gpu-driver")]
    public string? InstallGpuDriver { get; set; }

    [CliOption("--shielded-integrity-monitoring")]
    public string? ShieldedIntegrityMonitoring { get; set; }

    [CliOption("--shielded-secure-boot")]
    public string? ShieldedSecureBoot { get; set; }

    [CliOption("--shielded-vtpm")]
    public string? ShieldedVtpm { get; set; }
}