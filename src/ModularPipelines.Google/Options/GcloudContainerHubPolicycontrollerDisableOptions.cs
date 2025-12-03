using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "policycontroller", "disable")]
public record GcloudContainerHubPolicycontrollerDisableOptions : GcloudOptions
{
    [CliFlag("--fleet-default-member-config")]
    public bool? FleetDefaultMemberConfig { get; set; }

    [CliFlag("--all-memberships")]
    public bool? AllMemberships { get; set; }

    [CliOption("--memberships")]
    public string[]? Memberships { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}