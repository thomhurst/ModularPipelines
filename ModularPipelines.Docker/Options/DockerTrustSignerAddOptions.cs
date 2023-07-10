using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust signer add")]
public record DockerTrustSignerAddOptions : DockerOptions
{
    [CommandLongSwitch("key")]
    public string? Key { get; set; }

}