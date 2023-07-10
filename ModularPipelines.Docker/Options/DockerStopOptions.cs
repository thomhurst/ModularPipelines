using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stop")]
public record DockerStopOptions([property: PositionalArgument] string Container) : DockerOptions
{
    [CommandLongSwitch("signal")]
    public string? Signal { get; set; }

    [CommandLongSwitch("time")]
    public string? Time { get; set; }

}