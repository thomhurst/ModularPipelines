using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "identity-service", "delete")]
public record GcloudContainerHubIdentityServiceDeleteOptions : GcloudOptions
{
    [BooleanCommandSwitch("--fleet-default-member-config")]
    public bool? FleetDefaultMemberConfig { get; set; }

    [CommandSwitch("--membership")]
    public string? Membership { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}