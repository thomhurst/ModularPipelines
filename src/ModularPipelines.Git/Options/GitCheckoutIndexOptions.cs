using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("checkout-index")]
[ExcludeFromCodeCoverage]
public record GitCheckoutIndexOptions : GitOptions
{
    [BooleanCommandSwitch("--index")]
    public virtual bool? Index { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--no-create")]
    public virtual bool? NoCreate { get; set; }

    [CommandEqualsSeparatorSwitch("--prefix")]
    public string? Prefix { get; set; }

    [CommandEqualsSeparatorSwitch("--stage")]
    public string? Stage { get; set; }

    [BooleanCommandSwitch("--temp")]
    public virtual bool? Temp { get; set; }

    [BooleanCommandSwitch("--ignore-skip-worktree-bits")]
    public virtual bool? IgnoreSkipWorktreeBits { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public virtual bool? Stdin { get; set; }
}