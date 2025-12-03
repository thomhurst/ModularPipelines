using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "automation-runs", "list")]
public record GcloudDeployAutomationRunsListOptions : GcloudOptions
{
    [CliOption("--delivery-pipeline")]
    public string? DeliveryPipeline { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}