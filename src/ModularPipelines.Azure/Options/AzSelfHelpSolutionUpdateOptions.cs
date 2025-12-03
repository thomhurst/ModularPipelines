using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("self-help", "solution", "update")]
public record AzSelfHelpSolutionUpdateOptions(
[property: CliOption("--scope")] string Scope,
[property: CliOption("--solution-name")] string SolutionName
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--content")]
    public string? Content { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--replacement-maps")]
    public string? ReplacementMaps { get; set; }

    [CliOption("--sections")]
    public string? Sections { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--solution-id")]
    public string? SolutionId { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--trigger-criteria")]
    public string? TriggerCriteria { get; set; }
}