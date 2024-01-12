using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "create")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Tpu { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Zone { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }

    [CommandSwitch("--accelerator-type")]
    public string? AcceleratorType { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [BooleanCommandSwitch("--preemptible")]
    public bool? Preemptible { get; set; }

    [CommandSwitch("--range")]
    public string? Range { get; set; }
}
