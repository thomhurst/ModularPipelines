using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "automations", "export")]
public record GcloudDeployAutomationsExportOptions(
[property: PositionalArgument] string Automation,
[property: PositionalArgument] string DeliveryPipeline,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}