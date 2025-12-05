using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "delivery-pipelines", "delete")]
public record GcloudDeployDeliveryPipelinesDeleteOptions(
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }
}