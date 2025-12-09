using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "wiki", "page", "delete")]
public record AzDevopsWikiPageDeleteOptions(
[property: CliOption("--path")] string Path,
[property: CliOption("--wiki")] string Wiki
) : AzOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}