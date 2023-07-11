using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("ps")]
public record DockerPsOptions : DockerOptions
{
    [CommandSwitch("--last")]
    public int? Last { get; set; }

    [CommandSwitch("--latest")]
    public string? Latest { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public bool? NoTrunc { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }

}