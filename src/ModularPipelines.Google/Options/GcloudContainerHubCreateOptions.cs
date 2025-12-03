using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "create")]
public record GcloudContainerHubCreateOptions : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CliOption("--binauthz-policy-bindings")]
    public string[]? BinauthzPolicyBindings { get; set; }

    [CliOption("--security-posture")]
    public string? SecurityPosture { get; set; }

    [CliOption("--workload-vulnerability-scanning")]
    public string? WorkloadVulnerabilityScanning { get; set; }
}