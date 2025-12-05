using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "automations", "describe")]
public record GcloudDeployAutomationsDescribeOptions(
[property: CliArgument] string Automation,
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region
) : GcloudOptions;