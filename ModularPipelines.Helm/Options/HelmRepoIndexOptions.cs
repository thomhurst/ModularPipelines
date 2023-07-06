using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("repo", "index")]
public record HelmRepoIndexOptions : HelmOptions
{
    [CommandLongSwitch("merge", SwitchValueSeparator = " ")]
    public string? Merge { get; set; }

    [CommandLongSwitch("url", SwitchValueSeparator = " ")]
    public string? Url { get; set; }

}
