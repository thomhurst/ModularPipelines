using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack deploy")]
public record DockerStackDeployOptions : DockerOptions
{
    [CommandLongSwitch("prune")]
    public string? Prune { get; set; }

    [CommandLongSwitch("resolve-image")]
    public string? ResolveImage { get; set; }

    [CommandLongSwitch("with-registry-auth")]
    public string? WithRegistryAuth { get; set; }

}