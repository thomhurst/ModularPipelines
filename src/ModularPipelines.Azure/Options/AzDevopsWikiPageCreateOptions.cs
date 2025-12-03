using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops", "wiki", "page", "create")]
public record AzDevopsWikiPageCreateOptions(
[property: CliOption("--path")] string Path,
[property: CliOption("--wiki")] string Wiki
) : AzOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--content")]
    public string? Content { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--encoding")]
    public string? Encoding { get; set; }

    [CliOption("--file-path")]
    public string? FilePath { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}