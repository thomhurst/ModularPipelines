using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("worktree", "add")]
[ExcludeFromCodeCoverage]
public record GitWorktreeOptions : GitOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [BooleanCommandSwitch("--no-checkout")]
    public bool? NoCheckout { get; set; }

    [BooleanCommandSwitch("--checkout")]
    public bool? Checkout { get; set; }

    [BooleanCommandSwitch("--no-guess-remote")]
    public bool? NoGuessRemote { get; set; }

    [BooleanCommandSwitch("--guess-remote")]
    public bool? GuessRemote { get; set; }

    [BooleanCommandSwitch("--no-track")]
    public bool? NoTrack { get; set; }

    [BooleanCommandSwitch("--track")]
    public bool? Track { get; set; }

    [BooleanCommandSwitch("--lock")]
    public bool? Lock { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--porcelain")]
    public bool? Porcelain { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [CommandEqualsSeparatorSwitch("--expire")]
    public string? Expire { get; set; }

    [CommandEqualsSeparatorSwitch("--reason")]
    public string? Reason { get; set; }
}
