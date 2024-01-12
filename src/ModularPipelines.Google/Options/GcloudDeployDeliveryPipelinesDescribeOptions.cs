using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "delivery-pipelines", "describe")]
public record GcloudDeployDeliveryPipelinesDescribeOptions(
[property: PositionalArgument] string DeliveryPipeline,
[property: PositionalArgument] string Region
) : GcloudOptions;