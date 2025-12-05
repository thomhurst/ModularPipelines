using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "memberships", "bindings", "list")]
public record GcloudContainerFleetMembershipsBindingsListOptions(
[property: CliOption("--membership")] string Membership
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}