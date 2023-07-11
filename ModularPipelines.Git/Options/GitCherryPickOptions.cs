using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("cherry-pick")]
public record GitCherryPickOptions : GitOptions
{
    [BooleanCommandSwitch("--edit")]
    public bool? Edit { get; set; }

    [CommandEqualsSeparatorSwitch("--cleanup")]
    public string? Cleanup { get; set; }

    [CommandEqualsSeparatorSwitch("--mainline")]
    public string? Mainline { get; set; }

    [BooleanCommandSwitch("--no-commit")]
    public bool? NoCommit { get; set; }

    [BooleanCommandSwitch("--signoff")]
    public bool? Signoff { get; set; }

    [CommandEqualsSeparatorSwitch("--gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("--no-gpg-sign")]
    public bool? NoGpgSign { get; set; }

    [BooleanCommandSwitch("--ff")]
    public bool? Ff { get; set; }

    [BooleanCommandSwitch("--allow-empty")]
    public bool? AllowEmpty { get; set; }

    [BooleanCommandSwitch("--allow-empty-message")]
    public bool? AllowEmptyMessage { get; set; }

    [BooleanCommandSwitch("--keep-redundant-commits")]
    public bool? KeepRedundantCommits { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy")]
    public string? Strategy { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy-option")]
    public string? StrategyOption { get; set; }

    [BooleanCommandSwitch("--rerere-autoupdate")]
    public bool? RerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--no-rerere-autoupdate")]
    public bool? NoRerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--continue")]
    public bool? Continue { get; set; }

    [BooleanCommandSwitch("--skip")]
    public bool? Skip { get; set; }

    [BooleanCommandSwitch("--quit")]
    public bool? Quit { get; set; }

    [BooleanCommandSwitch("--abort")]
    public bool? Abort { get; set; }
}