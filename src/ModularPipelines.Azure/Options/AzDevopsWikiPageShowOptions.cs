using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "wiki", "page", "show")]
public record AzDevopsWikiPageShowOptions(
[property: CliOption("--path")] string Path,
[property: CliOption("--wiki")] string Wiki
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--include-content")]
    public bool? IncludeContent { get; set; }

    [CliFlag("--open")]
    public bool? Open { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}