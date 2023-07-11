using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("repo", "index")]
public record HelmRepoIndexOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--merge", SwitchValueSeparator = " ")]
    public string? Merge { get; set; }

    [CommandEqualsSeparatorSwitch("--url", SwitchValueSeparator = " ")]
    public string? Url { get; set; }
}
