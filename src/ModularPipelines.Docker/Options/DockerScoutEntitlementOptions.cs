using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout entitlement")]
[ExcludeFromCodeCoverage]
public record DockerScoutEntitlementOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Repository) : DockerOptions
{
    [CommandSwitch("--disable")]
    public string? Disable { get; set; }
}
