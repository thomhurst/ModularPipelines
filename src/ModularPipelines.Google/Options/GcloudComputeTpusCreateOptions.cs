using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "create")]
public record GcloudComputeTpusCreateOptions : GcloudOptions
{
    public GcloudComputeTpusCreateOptions(
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

    [CliOption("--accelerator-type")]
    public string? AcceleratorType { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliFlag("--preemptible")]
    public bool? Preemptible { get; set; }

    [CliOption("--range")]
    public string? Range { get; set; }
}
