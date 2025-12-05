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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Stack { get; set; }

    [CliOption("--compose-file")]
    public virtual string? ComposeFile { get; set; }

    [CliOption("--prune")]
    public virtual string? Prune { get; set; }

    [CliOption("--resolve-image")]
    public virtual string? ResolveImage { get; set; }

    [CliOption("--with-registry-auth")]
    public virtual string? WithRegistryAuth { get; set; }
}
