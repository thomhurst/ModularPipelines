using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "automation-runs", "cancel")]
public record GcloudDeployAutomationRunsCancelOptions(
[property: CliArgument] string AutomationRun,
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region
) : GcloudOptions;