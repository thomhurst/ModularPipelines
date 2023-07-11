using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("commit-tree")]
public record GitCommitTreeOptions : GitOptions
{
    [CommandEqualsSeparatorSwitch("--gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("--no-gpg-sign")]
    public bool? NoGpgSign { get; set; }

}