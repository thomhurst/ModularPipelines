using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "identity-service", "enable")]
public record GcloudContainerFleetIdentityServiceEnableOptions : GcloudOptions
{
    [CliOption("--fleet-default-member-config")]
    public string? FleetDefaultMemberConfig { get; set; }
}