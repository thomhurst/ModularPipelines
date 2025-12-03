using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workbench", "instances", "check-instance-upgradability")]
public record GcloudWorkbenchInstancesCheckInstanceUpgradabilityOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location
) : GcloudOptions;