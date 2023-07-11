using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("bundle")]
public record GitBundleOptions : GitOptions
{
    [BooleanCommandSwitch("--progress")]
    public bool? Progress { get; set; }

    [CommandEqualsSeparatorSwitch("--version")]
    public string? Version { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }
}