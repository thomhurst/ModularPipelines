using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "identity-service", "apply")]
public record GcloudContainerHubIdentityServiceApplyOptions(
[property: CliOption("--fleet-default-member-config")] string FleetDefaultMemberConfig,
[property: CliOption("--config")] string Config,
[property: CliOption("--origin")] string Origin,
[property: CliOption("--membership")] string Membership,
[property: CliOption("--location")] string Location
) : GcloudOptions;