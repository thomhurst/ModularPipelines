using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "execution-groups", "create")]
public record GcloudComputeTpusExecutionGroupsCreateOptions : GcloudOptions
{
    [CommandSwitch("--accelerator-type")]
    public string? AcceleratorType { get; set; }

    [CommandSwitch("--disk-size")]
    public string? DiskSize { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--forward-ports")]
    public bool? ForwardPorts { get; set; }

    [CommandSwitch("--gce-image")]
    public string? GceImage { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [BooleanCommandSwitch("--preemptible")]
    public bool? Preemptible { get; set; }

    [BooleanCommandSwitch("--preemptible-vm")]
    public bool? PreemptibleVm { get; set; }

    [CommandSwitch("--tf-version")]
    public string? TfVersion { get; set; }

    [BooleanCommandSwitch("--tpu-only")]
    public bool? TpuOnly { get; set; }

    [BooleanCommandSwitch("--use-dl-images")]
    public bool? UseDlImages { get; set; }

    [BooleanCommandSwitch("--use-with-notebook")]
    public bool? UseWithNotebook { get; set; }

    [BooleanCommandSwitch("--vm-only")]
    public bool? VmOnly { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}