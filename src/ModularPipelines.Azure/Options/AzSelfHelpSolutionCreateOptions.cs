using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("self-help", "solution", "create")]
public record AzSelfHelpSolutionCreateOptions(
[property: CliOption("--scope")] string Scope,
[property: CliOption("--solution-name")] string SolutionName
) : AzOptions
{
    [CliOption("--content")]
    public string? Content { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CliOption("--replacement-maps")]
    public string? ReplacementMaps { get; set; }

    [CliOption("--sections")]
    public string? Sections { get; set; }

    [CliOption("--solution-id")]
    public string? SolutionId { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--trigger-criteria")]
    public string? TriggerCriteria { get; set; }
}