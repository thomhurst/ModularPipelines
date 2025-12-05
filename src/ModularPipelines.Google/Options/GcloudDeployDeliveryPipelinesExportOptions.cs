using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "delivery-pipelines", "export")]
public record GcloudDeployDeliveryPipelinesExportOptions(
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}