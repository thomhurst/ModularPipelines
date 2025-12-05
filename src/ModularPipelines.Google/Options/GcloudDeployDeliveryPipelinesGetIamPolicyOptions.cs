using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "delivery-pipelines", "get-iam-policy")]
public record GcloudDeployDeliveryPipelinesGetIamPolicyOptions(
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region
) : GcloudOptions;