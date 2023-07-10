using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image pull")]
public record DockerImagePullOptions : DockerOptions
{
    [CommandLongSwitch("all-tags")]
    public string? AllTags { get; set; }

    [BooleanCommandSwitch("disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandLongSwitch("platform")]
    public string? Platform { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}