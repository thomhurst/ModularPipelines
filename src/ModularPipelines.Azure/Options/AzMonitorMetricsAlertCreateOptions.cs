using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "metrics", "alert", "create")]
public record AzMonitorMetricsAlertCreateOptions(
[property: CliOption("--condition")] string Condition,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--scopes")] string Scopes
) : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliFlag("--auto-mitigate")]
    public bool? AutoMitigate { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--evaluation-frequency")]
    public string? EvaluationFrequency { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--severity")]
    public string? Severity { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--target-resource-type")]
    public string? TargetResourceType { get; set; }

    [CliOption("--window-size")]
    public string? WindowSize { get; set; }
}