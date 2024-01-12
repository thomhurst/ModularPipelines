using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "identity-service", "enable")]
public record GcloudContainerHubIdentityServiceEnableOptions : GcloudOptions
{
    [CommandSwitch("--fleet-default-member-config")]
    public string? FleetDefaultMemberConfig { get; set; }
}