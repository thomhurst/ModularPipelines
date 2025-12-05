using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "policycontroller", "detach")]
public record GcloudContainerFleetPolicycontrollerDetachOptions : GcloudOptions
{
    [CliFlag("--all-memberships")]
    public bool? AllMemberships { get; set; }

    [CliOption("--memberships")]
    public string[]? Memberships { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}