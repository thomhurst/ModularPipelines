using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "automations", "list")]
public record GcloudDeployAutomationsListOptions : GcloudOptions
{
    [CliOption("--delivery-pipeline")]
    public string? DeliveryPipeline { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}