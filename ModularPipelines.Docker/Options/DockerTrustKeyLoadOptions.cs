using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust key load")]
public record DockerTrustKeyLoadOptions : DockerOptions
{
    [CommandLongSwitch("name")]
    public string? Name { get; set; }

}