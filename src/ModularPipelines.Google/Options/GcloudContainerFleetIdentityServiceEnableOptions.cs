using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "identity-service", "enable")]
public record GcloudContainerFleetIdentityServiceEnableOptions : GcloudOptions
{
    [CommandSwitch("--fleet-default-member-config")]
    public string? FleetDefaultMemberConfig { get; set; }
}