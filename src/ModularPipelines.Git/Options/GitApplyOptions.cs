using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("apply")]
[ExcludeFromCodeCoverage]
public record GitApplyOptions : GitOptions
{
    [CliFlag("--stat")]
    public virtual bool? Stat { get; set; }

    [CliFlag("--numstat")]
    public virtual bool? Numstat { get; set; }

    [CliFlag("--summary")]
    public virtual bool? Summary { get; set; }

    [CliFlag("--check")]
    public virtual bool? Check { get; set; }

    [CliFlag("--index")]
    public virtual bool? Index { get; set; }

    [CliFlag("--cached")]
    public virtual bool? Cached { get; set; }

    [CliFlag("--intent-to-add")]
    public virtual bool? IntentToAdd { get; set; }

    [CliFlag("--3way")]
    public virtual bool? ThreeWay { get; set; }

    [CliOption("--build-fake-ancestor", Format = OptionFormat.EqualsSeparated)]
    public virtual string? BuildFakeAncestor { get; set; }

    [CliFlag("--reverse")]
    public virtual bool? Reverse { get; set; }

    [CliFlag("--reject")]
    public virtual bool? Reject { get; set; }

    [CliFlag("--unidiff-zero")]
    public virtual bool? UnidiffZero { get; set; }

    [CliFlag("--apply")]
    public virtual bool? Apply { get; set; }

    [CliFlag("--no-add")]
    public virtual bool? NoAdd { get; set; }

    [CliFlag("--allow-binary-replacement")]
    public virtual bool? AllowBinaryReplacement { get; set; }

    [CliFlag("--binary")]
    public virtual bool? Binary { get; set; }

    [CliOption("--exclude", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Exclude { get; set; }

    [CliOption("--include", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Include { get; set; }

    [CliFlag("--ignore-space-change")]
    public virtual bool? IgnoreSpaceChange { get; set; }

    [CliFlag("--ignore-whitespace")]
    public virtual bool? IgnoreWhitespace { get; set; }

    [CliOption("--whitespace", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Whitespace { get; set; }

    [CliFlag("--inaccurate-eof")]
    public virtual bool? InaccurateEof { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--recount")]
    public virtual bool? Recount { get; set; }

    [CliOption("--directory", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Directory { get; set; }

    [CliFlag("--unsafe-paths")]
    public virtual bool? UnsafePaths { get; set; }

    [CliFlag("--allow-empty")]
    public virtual bool? AllowEmpty { get; set; }
}