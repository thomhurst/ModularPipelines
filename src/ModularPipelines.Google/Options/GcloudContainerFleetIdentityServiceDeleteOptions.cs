using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "identity-service", "delete")]
public record GcloudContainerFleetIdentityServiceDeleteOptions : GcloudOptions
{
    [CliFlag("--fleet-default-member-config")]
    public bool? FleetDefaultMemberConfig { get; set; }

    [CliOption("--membership")]
    public string? Membership { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}