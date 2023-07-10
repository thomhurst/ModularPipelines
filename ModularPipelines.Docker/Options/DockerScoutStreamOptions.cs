using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout stream")]
public record DockerScoutStreamOptions : DockerOptions
{
    [CommandLongSwitch("app")]
    public string? App { get; set; }

    [CommandLongSwitch("platform")]
    public string? Platform { get; set; }

}