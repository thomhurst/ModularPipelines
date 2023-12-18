using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scenario", "guide")]
public record AzScenarioGuideOptions : AzOptions
{
    [CommandSwitch("--match-rule")]
    public string? MatchRule { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? SearchKeyword { get; set; }
}