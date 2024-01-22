using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("login")]
[ExcludeFromCodeCoverage]
public record DockerLoginOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Server { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--password-stdin")]
    public string? PasswordStdin { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}
