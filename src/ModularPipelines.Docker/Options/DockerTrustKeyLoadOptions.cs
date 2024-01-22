using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerTrustKeyLoadOptions : DockerOptions
{
    public DockerTrustKeyLoadOptions(
        string keyfile
    )
    {
        CommandParts = ["trust", "key", "load"];

        Keyfile = keyfile;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Keyfile { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }
}
