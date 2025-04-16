using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("apply")]
[ExcludeFromCodeCoverage]
public record GitApplyOptions : GitOptions
{
    [BooleanCommandSwitch("--stat")]
    public virtual bool? Stat { get; set; }

    [BooleanCommandSwitch("--numstat")]
    public virtual bool? Numstat { get; set; }

    [BooleanCommandSwitch("--summary")]
    public virtual bool? Summary { get; set; }

    [BooleanCommandSwitch("--check")]
    public virtual bool? Check { get; set; }

    [BooleanCommandSwitch("--index")]
    public virtual bool? Index { get; set; }

    [BooleanCommandSwitch("--cached")]
    public virtual bool? Cached { get; set; }

    [BooleanCommandSwitch("--intent-to-add")]
    public virtual bool? IntentToAdd { get; set; }

    [BooleanCommandSwitch("--3way")]
    public virtual bool? ThreeWay { get; set; }

    [CommandEqualsSeparatorSwitch("--build-fake-ancestor")]
    public string? BuildFakeAncestor { get; set; }

    [BooleanCommandSwitch("--reverse")]
    public virtual bool? Reverse { get; set; }

    [BooleanCommandSwitch("--reject")]
    public virtual bool? Reject { get; set; }

    [BooleanCommandSwitch("--unidiff-zero")]
    public virtual bool? UnidiffZero { get; set; }

    [BooleanCommandSwitch("--apply")]
    public virtual bool? Apply { get; set; }

    [BooleanCommandSwitch("--no-add")]
    public virtual bool? NoAdd { get; set; }

    [BooleanCommandSwitch("--allow-binary-replacement")]
    public virtual bool? AllowBinaryReplacement { get; set; }

    [BooleanCommandSwitch("--binary")]
    public virtual bool? Binary { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude")]
    public string? Exclude { get; set; }

    [CommandEqualsSeparatorSwitch("--include")]
    public string? Include { get; set; }

    [BooleanCommandSwitch("--ignore-space-change")]
    public virtual bool? IgnoreSpaceChange { get; set; }

    [BooleanCommandSwitch("--ignore-whitespace")]
    public virtual bool? IgnoreWhitespace { get; set; }

    [CommandEqualsSeparatorSwitch("--whitespace")]
    public string? Whitespace { get; set; }

    [BooleanCommandSwitch("--inaccurate-eof")]
    public virtual bool? InaccurateEof { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--recount")]
    public virtual bool? Recount { get; set; }

    [CommandEqualsSeparatorSwitch("--directory")]
    public string? Directory { get; set; }

    [BooleanCommandSwitch("--unsafe-paths")]
    public virtual bool? UnsafePaths { get; set; }

    [BooleanCommandSwitch("--allow-empty")]
    public virtual bool? AllowEmpty { get; set; }
}