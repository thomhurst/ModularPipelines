using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "delivery-pipelines", "set-iam-policy")]
public record GcloudDeployDeliveryPipelinesSetIamPolicyOptions(
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions;