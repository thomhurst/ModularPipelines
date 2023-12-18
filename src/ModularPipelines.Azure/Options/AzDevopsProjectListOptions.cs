using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "project", "list")]
public record AzDevopsProjectListOptions(
[property: CommandSwitch("--project")] string Project
) : AzOptions
{
    [CommandSwitch("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [BooleanCommandSwitch("--get-default-team-image-url")]
    public bool? GetDefaultTeamImageUrl { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--skip")]
    public string? Skip { get; set; }

    [CommandSwitch("--state-filter")]
    public string? StateFilter { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}

