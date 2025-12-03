using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scenario", "guide")]
public record AzScenarioGuideOptions : AzOptions
{
    [CliOption("--match-rule")]
    public string? MatchRule { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? SEARCHKEYWORD { get; set; }
}