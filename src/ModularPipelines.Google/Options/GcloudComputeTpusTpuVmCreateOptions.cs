using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "tpu-vm", "create")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Tpu { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Zone { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--data-disk")]
    public string[]? DataDisk { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--internal-ips")]
    public bool? InternalIps { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--metadata-from-file")]
    public IEnumerable<KeyValue>? MetadataFromFile { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliFlag("--preemptible")]
    public bool? Preemptible { get; set; }

    [CliOption("--range")]
    public string? Range { get; set; }

    [CliFlag("--reserved")]
    public bool? Reserved { get; set; }

    [CliOption("--scopes")]
    public string[]? Scopes { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliFlag("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [CliOption("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--accelerator-type")]
    public string? AcceleratorType { get; set; }

    [CliOption("--topology")]
    public string? Topology { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}
