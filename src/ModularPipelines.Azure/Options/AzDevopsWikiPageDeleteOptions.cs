using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "wiki", "page", "delete")]
public record AzDevopsWikiPageDeleteOptions(
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--wiki")] string Wiki
) : AzOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}