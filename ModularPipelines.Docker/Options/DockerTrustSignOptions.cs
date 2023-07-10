using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust sign")]
public record DockerTrustSignOptions : DockerOptions
{
    [CommandLongSwitch("local")]
    public string? Local { get; set; }

}