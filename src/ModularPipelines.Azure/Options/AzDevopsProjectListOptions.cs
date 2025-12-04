using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "project", "list")]
public record AzDevopsProjectListOptions : AzOptions
{
    [CliOption("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--get-default-team-image-url")]
    public bool? GetDefaultTeamImageUrl { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--skip")]
    public string? Skip { get; set; }

    [CliOption("--state-filter")]
    public string? StateFilter { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}