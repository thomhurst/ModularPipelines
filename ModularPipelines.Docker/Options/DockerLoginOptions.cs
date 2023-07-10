using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("login")]
public record DockerLoginOptions : DockerOptions
{
    [CommandLongSwitch("password")]
    public string? Password { get; set; }

    [CommandLongSwitch("username")]
    public string? Username { get; set; }

}