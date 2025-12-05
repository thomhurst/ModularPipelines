using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "delivery-pipelines", "describe")]
public record GcloudDeployDeliveryPipelinesDescribeOptions(
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region
) : GcloudOptions;