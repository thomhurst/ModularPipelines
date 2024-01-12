using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "identity-service", "apply")]
public record GcloudContainerHubIdentityServiceApplyOptions(
[property: CommandSwitch("--fleet-default-member-config")] string FleetDefaultMemberConfig,
[property: CommandSwitch("--config")] string Config,
[property: CommandSwitch("--origin")] string Origin,
[property: CommandSwitch("--membership")] string Membership,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;