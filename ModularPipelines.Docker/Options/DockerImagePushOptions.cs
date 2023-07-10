using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image push")]
public record DockerImagePushOptions : DockerOptions
{
    [CommandLongSwitch("all-tags")]
    public string? AllTags { get; set; }

    [BooleanCommandSwitch("disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}