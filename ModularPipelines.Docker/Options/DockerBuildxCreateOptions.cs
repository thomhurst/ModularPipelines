using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx create")]
public record DockerBuildxCreateOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string Context { get; set; }
    [CommandSwitch("--bootstrap")]
    public string? Bootstrap { get; set; }

    [CommandSwitch("--append")]
    public string? Append { get; set; }

    [CommandSwitch("--buildkitd-flags")]
    public string? BuildkitdFlags { get; set; }

    [CommandSwitch("--config")]
    public string? Config { get; set; }

    [CommandSwitch("--driver")]
    public string? Driver { get; set; }

    [CommandSwitch("--driver-opt")]
    public string? DriverOpt { get; set; }

    [CommandSwitch("--leave")]
    public string? Leave { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--node")]
    public string? Node { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--use")]
    public string? Use { get; set; }

}