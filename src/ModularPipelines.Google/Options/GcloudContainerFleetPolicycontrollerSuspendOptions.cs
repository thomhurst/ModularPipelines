using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "policycontroller", "suspend")]
public record GcloudContainerFleetPolicycontrollerSuspendOptions : GcloudOptions
{
    [BooleanCommandSwitch("--all-memberships")]
    public bool? AllMemberships { get; set; }

    [CommandSwitch("--memberships")]
    public string[]? Memberships { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}