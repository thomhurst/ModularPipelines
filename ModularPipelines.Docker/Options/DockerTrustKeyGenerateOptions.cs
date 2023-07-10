using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust key generate")]
public record DockerTrustKeyGenerateOptions : DockerOptions
{
    [CommandLongSwitch("dir")]
    public string? Dir { get; set; }

}