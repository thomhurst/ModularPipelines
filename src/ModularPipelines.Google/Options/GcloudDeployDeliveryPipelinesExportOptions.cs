using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "delivery-pipelines", "export")]
public record GcloudDeployDeliveryPipelinesExportOptions(
[property: PositionalArgument] string DeliveryPipeline,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}