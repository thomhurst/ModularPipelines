using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "automation-runs", "list")]
public record GcloudDeployAutomationRunsListOptions : GcloudOptions
{
    [CommandSwitch("--delivery-pipeline")]
    public string? DeliveryPipeline { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}