using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("push")]
public record DockerPushOptions([property: PositionalArgument] string Name) : DockerOptions
{
    [BooleanCommandSwitch("disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}