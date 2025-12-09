using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("cherry-pick")]
[ExcludeFromCodeCoverage]
public record GitCherryPickOptions : GitOptions
{
    [CliFlag("--edit")]
    public virtual bool? Edit { get; set; }

    [CliOption("--cleanup", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Cleanup { get; set; }

    [CliOption("--mainline", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Mainline { get; set; }

    [CliFlag("--no-commit")]
    public virtual bool? NoCommit { get; set; }

    [CliFlag("--signoff")]
    public virtual bool? Signoff { get; set; }

    [CliOption("--gpg-sign", Format = OptionFormat.EqualsSeparated)]
    public virtual string? GpgSign { get; set; }

    [CliFlag("--no-gpg-sign")]
    public virtual bool? NoGpgSign { get; set; }

    [CliFlag("--ff")]
    public virtual bool? Ff { get; set; }

    [CliFlag("--allow-empty")]
    public virtual bool? AllowEmpty { get; set; }

    [CliFlag("--allow-empty-message")]
    public virtual bool? AllowEmptyMessage { get; set; }

    [CliFlag("--keep-redundant-commits")]
    public virtual bool? KeepRedundantCommits { get; set; }

    [CliOption("--strategy", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Strategy { get; set; }

    [CliOption("--strategy-option", Format = OptionFormat.EqualsSeparated)]
    public virtual string? StrategyOption { get; set; }

    [CliFlag("--rerere-autoupdate")]
    public virtual bool? RerereAutoupdate { get; set; }

    [CliFlag("--no-rerere-autoupdate")]
    public virtual bool? NoRerereAutoupdate { get; set; }

    [CliFlag("--continue")]
    public virtual bool? Continue { get; set; }

    [CliFlag("--skip")]
    public virtual bool? Skip { get; set; }

    [CliFlag("--quit")]
    public virtual bool? Quit { get; set; }

    [CliFlag("--abort")]
    public virtual bool? Abort { get; set; }
}