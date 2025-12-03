using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("revert")]
[ExcludeFromCodeCoverage]
public record GitRevertOptions : GitOptions
{
    [CliFlag("--edit")]
    public virtual bool? Edit { get; set; }

    [CliFlag("--mainline")]
    public virtual bool? Mainline { get; set; }

    [CliFlag("--no-edit")]
    public virtual bool? NoEdit { get; set; }

    [CliOption("--cleanup", Format = OptionFormat.EqualsSeparated)]
    public string? Cleanup { get; set; }

    [CliFlag("--no-commit")]
    public virtual bool? NoCommit { get; set; }

    [CliOption("--gpg-sign", Format = OptionFormat.EqualsSeparated)]
    public string? GpgSign { get; set; }

    [CliFlag("--no-gpg-sign")]
    public virtual bool? NoGpgSign { get; set; }

    [CliFlag("--signoff")]
    public virtual bool? Signoff { get; set; }

    [CliOption("--strategy", Format = OptionFormat.EqualsSeparated)]
    public string? Strategy { get; set; }

    [CliOption("--strategy-option", Format = OptionFormat.EqualsSeparated)]
    public string? StrategyOption { get; set; }

    [CliFlag("--rerere-autoupdate")]
    public virtual bool? RerereAutoupdate { get; set; }

    [CliFlag("--no-rerere-autoupdate")]
    public virtual bool? NoRerereAutoupdate { get; set; }

    [CliFlag("--reference")]
    public virtual bool? Reference { get; set; }

    [CliFlag("--continue")]
    public virtual bool? Continue { get; set; }

    [CliFlag("--skip")]
    public virtual bool? Skip { get; set; }

    [CliFlag("--quit")]
    public virtual bool? Quit { get; set; }

    [CliFlag("--abort")]
    public virtual bool? Abort { get; set; }
}