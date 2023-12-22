using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("revert")]
[ExcludeFromCodeCoverage]
public record GitRevertOptions : GitOptions
{
    [BooleanCommandSwitch("--edit")]
    public bool? Edit { get; set; }

    [BooleanCommandSwitch("--mainline")]
    public bool? Mainline { get; set; }

    [BooleanCommandSwitch("--no-edit")]
    public bool? NoEdit { get; set; }

    [CommandEqualsSeparatorSwitch("--cleanup")]
    public string? Cleanup { get; set; }

    [BooleanCommandSwitch("--no-commit")]
    public bool? NoCommit { get; set; }

    [CommandEqualsSeparatorSwitch("--gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("--no-gpg-sign")]
    public bool? NoGpgSign { get; set; }

    [BooleanCommandSwitch("--signoff")]
    public bool? Signoff { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy")]
    public string? Strategy { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy-option")]
    public string? StrategyOption { get; set; }

    [BooleanCommandSwitch("--rerere-autoupdate")]
    public bool? RerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--no-rerere-autoupdate")]
    public bool? NoRerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--reference")]
    public bool? Reference { get; set; }

    [BooleanCommandSwitch("--continue")]
    public bool? Continue { get; set; }

    [BooleanCommandSwitch("--skip")]
    public bool? Skip { get; set; }

    [BooleanCommandSwitch("--quit")]
    public bool? Quit { get; set; }

    [BooleanCommandSwitch("--abort")]
    public bool? Abort { get; set; }
}