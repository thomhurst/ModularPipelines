using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "tpu-vm", "create")]
public record GcloudComputeTpusTpuVmCreateOptions : GcloudOptions
{
    public GcloudComputeTpusTpuVmCreateOptions(
        string tpu,
        string zone,
        string version
    )
    {
        Tpu = tpu;
        Zone = zone;
        Version = version;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Tpu { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Zone { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--data-disk")]
    public string[]? DataDisk { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--internal-ips")]
    public bool? InternalIps { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--metadata-from-file")]
    public IEnumerable<KeyValue>? MetadataFromFile { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [BooleanCommandSwitch("--preemptible")]
    public bool? Preemptible { get; set; }

    [CommandSwitch("--range")]
    public string? Range { get; set; }

    [BooleanCommandSwitch("--reserved")]
    public bool? Reserved { get; set; }

    [CommandSwitch("--scopes")]
    public string[]? Scopes { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [BooleanCommandSwitch("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [CommandSwitch("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--accelerator-type")]
    public string? AcceleratorType { get; set; }

    [CommandSwitch("--topology")]
    public string? Topology { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}
