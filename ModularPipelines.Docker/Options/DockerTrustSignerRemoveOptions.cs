using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust signer remove")]
public record DockerTrustSignerRemoveOptions : DockerOptions
{
    [CommandLongSwitch("force")]
    public string? Force { get; set; }

}