using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "automations", "describe")]
public record GcloudDeployAutomationsDescribeOptions(
[property: PositionalArgument] string Automation,
[property: PositionalArgument] string DeliveryPipeline,
[property: PositionalArgument] string Region
) : GcloudOptions;