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
    public virtual string? ComposeFile { get; set; }

    [CommandSwitch("--prune")]
    public virtual string? Prune { get; set; }

    [CommandSwitch("--resolve-image")]
    public virtual string? ResolveImage { get; set; }

    [CommandSwitch("--with-registry-auth")]
    public virtual string? WithRegistryAuth { get; set; }
}
