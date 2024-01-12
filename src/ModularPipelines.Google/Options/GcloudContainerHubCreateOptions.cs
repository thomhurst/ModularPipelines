using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "create")]
public record GcloudContainerHubCreateOptions : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CommandSwitch("--binauthz-policy-bindings")]
    public string[]? BinauthzPolicyBindings { get; set; }

    [CommandSwitch("--security-posture")]
    public string? SecurityPosture { get; set; }

    [CommandSwitch("--workload-vulnerability-scanning")]
    public string? WorkloadVulnerabilityScanning { get; set; }
}