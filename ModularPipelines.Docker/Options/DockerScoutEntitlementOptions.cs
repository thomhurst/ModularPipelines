using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout entitlement")]
public record DockerScoutEntitlementOptions : DockerOptions
{
    [CommandLongSwitch("disable")]
    public string? Disable { get; set; }

}