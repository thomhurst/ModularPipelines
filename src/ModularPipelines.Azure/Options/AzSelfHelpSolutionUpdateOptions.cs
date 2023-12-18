using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("self-help", "solution", "update")]
public record AzSelfHelpSolutionUpdateOptions(
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--solution-name")] string SolutionName
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--content")]
    public string? Content { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--parameters")]
    public string? Parameters { get; set; }

    [CommandSwitch("--provisioning-state")]
    public string? ProvisioningState { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--replacement-maps")]
    public string? ReplacementMaps { get; set; }

    [CommandSwitch("--sections")]
    public string? Sections { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--solution-id")]
    public string? SolutionId { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }

    [CommandSwitch("--trigger-criteria")]
    public string? TriggerCriteria { get; set; }
}

