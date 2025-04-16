using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("worktree", "add")]
[ExcludeFromCodeCoverage]
public record GitWorktreeOptions : GitOptions
{
    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--detach")]
    public virtual bool? Detach { get; set; }

    [BooleanCommandSwitch("--no-checkout")]
    public virtual bool? NoCheckout { get; set; }

    [BooleanCommandSwitch("--checkout")]
    public virtual bool? Checkout { get; set; }

    [BooleanCommandSwitch("--no-guess-remote")]
    public virtual bool? NoGuessRemote { get; set; }

    [BooleanCommandSwitch("--guess-remote")]
    public virtual bool? GuessRemote { get; set; }

    [BooleanCommandSwitch("--no-track")]
    public virtual bool? NoTrack { get; set; }

    [BooleanCommandSwitch("--track")]
    public virtual bool? Track { get; set; }

    [BooleanCommandSwitch("--lock")]
    public virtual bool? Lock { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--porcelain")]
    public virtual bool? Porcelain { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CommandEqualsSeparatorSwitch("--expire")]
    public string? Expire { get; set; }

    [CommandEqualsSeparatorSwitch("--reason")]
    public string? Reason { get; set; }
}