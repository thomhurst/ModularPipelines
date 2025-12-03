using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "policycontroller", "deployment", "set")]
public record GcloudContainerHubPolicycontrollerDeploymentSetOptions(
[property: CliArgument] string Deployment,
[property: CliArgument] string Property,
[property: CliArgument] string Value
) : GcloudOptions
{
    [CliOption("--effect")]
    public string? Effect { get; set; }

    [CliFlag("--all-memberships")]
    public bool? AllMemberships { get; set; }

    [CliOption("--memberships")]
    public string[]? Memberships { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}