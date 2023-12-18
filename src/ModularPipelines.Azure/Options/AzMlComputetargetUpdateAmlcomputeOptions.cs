using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "computetarget", "update", "amlcompute")]
public record AzMlComputetargetUpdateAmlcomputeOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--idle-seconds-before-scaledown")]
    public string? IdleSecondsBeforeScaledown { get; set; }

    [CommandSwitch("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CommandSwitch("--min-nodes")]
    public string? MinNodes { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [BooleanCommandSwitch("-v")]
    public bool? V { get; set; }
}