using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("secret inspect")]
[ExcludeFromCodeCoverage]
public record DockerSecretInspectOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Secret) : DockerOptions
{
    [CommandSwitch("--pretty")]
    public string? Pretty { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}