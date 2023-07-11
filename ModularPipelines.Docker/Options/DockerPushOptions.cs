using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("push")]
public record DockerPushOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string Name { get; set; }
    [BooleanCommandSwitch("--disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--all-tags")]
    public bool? AllTags { get; set; }
}