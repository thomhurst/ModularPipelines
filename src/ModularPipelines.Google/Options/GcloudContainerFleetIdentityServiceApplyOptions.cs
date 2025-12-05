using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "identity-service", "apply")]
public record GcloudContainerFleetIdentityServiceApplyOptions(
[property: CliOption("--fleet-default-member-config")] string FleetDefaultMemberConfig,
[property: CliOption("--config")] string Config,
[property: CliOption("--origin")] string Origin,
[property: CliOption("--membership")] string Membership,
[property: CliOption("--location")] string Location
) : GcloudOptions;