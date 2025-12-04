using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("checkout-index")]
[ExcludeFromCodeCoverage]
public record GitCheckoutIndexOptions : GitOptions
{
    [CliFlag("--index")]
    public virtual bool? Index { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--no-create")]
    public virtual bool? NoCreate { get; set; }

    [CliOption("--prefix", Format = OptionFormat.EqualsSeparated)]
    public string? Prefix { get; set; }

    [CliOption("--stage", Format = OptionFormat.EqualsSeparated)]
    public string? Stage { get; set; }

    [CliFlag("--temp")]
    public virtual bool? Temp { get; set; }

    [CliFlag("--ignore-skip-worktree-bits")]
    public virtual bool? IgnoreSkipWorktreeBits { get; set; }

    [CliFlag("--stdin")]
    public virtual bool? Stdin { get; set; }
}