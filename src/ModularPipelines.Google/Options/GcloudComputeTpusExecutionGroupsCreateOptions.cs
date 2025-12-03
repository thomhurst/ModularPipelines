using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "execution-groups", "create")]
public record GcloudComputeTpusExecutionGroupsCreateOptions : GcloudOptions
{
    [CliOption("--accelerator-type")]
    public string? AcceleratorType { get; set; }

    [CliOption("--disk-size")]
    public string? DiskSize { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliFlag("--forward-ports")]
    public bool? ForwardPorts { get; set; }

    [CliOption("--gce-image")]
    public string? GceImage { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliFlag("--preemptible")]
    public bool? Preemptible { get; set; }

    [CliFlag("--preemptible-vm")]
    public bool? PreemptibleVm { get; set; }

    [CliOption("--tf-version")]
    public string? TfVersion { get; set; }

    [CliFlag("--tpu-only")]
    public bool? TpuOnly { get; set; }

    [CliFlag("--use-dl-images")]
    public bool? UseDlImages { get; set; }

    [CliFlag("--use-with-notebook")]
    public bool? UseWithNotebook { get; set; }

    [CliFlag("--vm-only")]
    public bool? VmOnly { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}