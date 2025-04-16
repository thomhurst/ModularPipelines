using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("revert")]
[ExcludeFromCodeCoverage]
public record GitRevertOptions : GitOptions
{
    [BooleanCommandSwitch("--edit")]
    public virtual bool? Edit { get; set; }

    [BooleanCommandSwitch("--mainline")]
    public virtual bool? Mainline { get; set; }

    [BooleanCommandSwitch("--no-edit")]
    public virtual bool? NoEdit { get; set; }

    [CommandEqualsSeparatorSwitch("--cleanup")]
    public string? Cleanup { get; set; }

    [BooleanCommandSwitch("--no-commit")]
    public virtual bool? NoCommit { get; set; }

    [CommandEqualsSeparatorSwitch("--gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("--no-gpg-sign")]
    public virtual bool? NoGpgSign { get; set; }

    [BooleanCommandSwitch("--signoff")]
    public virtual bool? Signoff { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy")]
    public string? Strategy { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy-option")]
    public string? StrategyOption { get; set; }

    [BooleanCommandSwitch("--rerere-autoupdate")]
    public virtual bool? RerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--no-rerere-autoupdate")]
    public virtual bool? NoRerereAutoupdate { get; set; }

    [BooleanCommandSwitch("--reference")]
    public virtual bool? Reference { get; set; }

    [BooleanCommandSwitch("--continue")]
    public virtual bool? Continue { get; set; }

    [BooleanCommandSwitch("--skip")]
    public virtual bool? Skip { get; set; }

    [BooleanCommandSwitch("--quit")]
    public virtual bool? Quit { get; set; }

    [BooleanCommandSwitch("--abort")]
    public virtual bool? Abort { get; set; }
}