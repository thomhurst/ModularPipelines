using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image pull")]
public record DockerImagePullOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string Name { get; set; }
    [BooleanCommandSwitch("--all-tags")]
    public bool? AllTags { get; set; }

    [BooleanCommandSwitch("--disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

}