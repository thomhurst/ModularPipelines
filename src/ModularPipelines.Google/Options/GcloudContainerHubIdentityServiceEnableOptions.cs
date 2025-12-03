using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "identity-service", "enable")]
public record GcloudContainerHubIdentityServiceEnableOptions : GcloudOptions
{
    [CliOption("--fleet-default-member-config")]
    public string? FleetDefaultMemberConfig { get; set; }
}