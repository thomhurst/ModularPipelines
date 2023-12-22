using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

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
    public string? SEARCHKEYWORD { get; set; }
}