using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("login")]
public record DockerLoginOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? Server { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--password-stdin")]
    public string? PasswordStdin { get; set; }
}