using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "memberships", "bindings", "list")]
public record GcloudContainerHubMembershipsBindingsListOptions(
[property: CliOption("--membership")] string Membership
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}