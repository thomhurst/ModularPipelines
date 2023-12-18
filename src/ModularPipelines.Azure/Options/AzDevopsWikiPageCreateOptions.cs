using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "wiki", "page", "create")]
public record AzDevopsWikiPageCreateOptions(
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--wiki")] string Wiki
) : AzOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--content")]
    public string? Content { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--encoding")]
    public string? Encoding { get; set; }

    [CommandSwitch("--file-path")]
    public string? FilePath { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}

