using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workbench", "instances", "check-instance-upgradability")]
public record GcloudWorkbenchInstancesCheckInstanceUpgradabilityOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Location
) : GcloudOptions;