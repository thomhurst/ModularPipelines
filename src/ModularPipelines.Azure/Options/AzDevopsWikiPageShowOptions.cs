using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "wiki", "page", "show")]
public record AzDevopsWikiPageShowOptions(
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--wiki")] string Wiki
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [BooleanCommandSwitch("--include-content")]
    public bool? IncludeContent { get; set; }

    [BooleanCommandSwitch("--open")]
    public bool? Open { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}