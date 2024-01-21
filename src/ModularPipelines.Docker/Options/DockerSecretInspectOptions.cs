using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerSecretInspectOptions : DockerOptions
{
    public DockerSecretInspectOptions(
        IEnumerable<string> secret
    )
    {
        CommandParts = ["secret", "inspect"];

        Secret = secret;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Secret { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--pretty")]
    public string? Pretty { get; set; }
}
