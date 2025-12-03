using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "automations", "export")]
public record GcloudDeployAutomationsExportOptions(
[property: CliArgument] string Automation,
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}