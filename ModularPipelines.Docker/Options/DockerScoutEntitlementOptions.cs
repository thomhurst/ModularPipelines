using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout entitlement")]
public record DockerScoutEntitlementOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Repository) : DockerOptions
{
    [CommandSwitch("--disable")]
    public string? Disable { get; set; }
}