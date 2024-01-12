using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "policycontroller", "disable")]
public record GcloudContainerHubPolicycontrollerDisableOptions : GcloudOptions
{
    [BooleanCommandSwitch("--fleet-default-member-config")]
    public bool? FleetDefaultMemberConfig { get; set; }

    [BooleanCommandSwitch("--all-memberships")]
    public bool? AllMemberships { get; set; }

    [CommandSwitch("--memberships")]
    public string[]? Memberships { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}