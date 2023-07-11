using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack deploy")]
public record DockerStackDeployOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Stack) : DockerOptions
{

    [CommandSwitch("--prune")]
    public string? Prune { get; set; }


    [CommandSwitch("--resolve-image")]
    public string? ResolveImage { get; set; }


    [CommandSwitch("--with-registry-auth")]
    public string? WithRegistryAuth { get; set; }


    [CommandSwitch("--compose-file")]
    public string? ComposeFile { get; set; }

}