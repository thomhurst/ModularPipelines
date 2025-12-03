using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("worktree", "add")]
[ExcludeFromCodeCoverage]
public record GitWorktreeOptions : GitOptions
{
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

    [CliFlag("--no-checkout")]
    public virtual bool? NoCheckout { get; set; }

    [CliFlag("--checkout")]
    public virtual bool? Checkout { get; set; }

    [CliFlag("--no-guess-remote")]
    public virtual bool? NoGuessRemote { get; set; }

    [CliFlag("--guess-remote")]
    public virtual bool? GuessRemote { get; set; }

    [CliFlag("--no-track")]
    public virtual bool? NoTrack { get; set; }

    [CliFlag("--track")]
    public virtual bool? Track { get; set; }

    [CliFlag("--lock")]
    public virtual bool? Lock { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--porcelain")]
    public virtual bool? Porcelain { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliOption("--expire", Format = OptionFormat.EqualsSeparated)]
    public string? Expire { get; set; }

    [CliOption("--reason", Format = OptionFormat.EqualsSeparated)]
    public string? Reason { get; set; }
}