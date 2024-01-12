using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "memberships", "bindings", "list")]
public record GcloudContainerHubMembershipsBindingsListOptions(
[property: CommandSwitch("--membership")] string Membership
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}