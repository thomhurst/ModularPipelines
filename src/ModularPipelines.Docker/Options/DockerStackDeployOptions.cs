using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerStackDeployOptions : DockerOptions
{
    public DockerStackDeployOptions(
        string stack
    )
    {
        CommandParts = ["stack", "deploy"];

        Stack = stack;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Stack { get; set; }

    [CommandSwitch("--compose-file")]
    public string? ComposeFile { get; set; }

    [CommandSwitch("--prune")]
    public string? Prune { get; set; }

    [CommandSwitch("--resolve-image")]
    public string? ResolveImage { get; set; }

    [CommandSwitch("--with-registry-auth")]
    public string? WithRegistryAuth { get; set; }
}
