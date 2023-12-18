using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("apply")]
[ExcludeFromCodeCoverage]
public record GitApplyOptions : GitOptions
{
    [BooleanCommandSwitch("--stat")]
    public bool? Stat { get; set; }

    [BooleanCommandSwitch("--numstat")]
    public bool? Numstat { get; set; }

    [BooleanCommandSwitch("--summary")]
    public bool? Summary { get; set; }

    [BooleanCommandSwitch("--check")]
    public bool? Check { get; set; }

    [BooleanCommandSwitch("--index")]
    public bool? Index { get; set; }

    [BooleanCommandSwitch("--cached")]
    public bool? Cached { get; set; }

    [BooleanCommandSwitch("--intent-to-add")]
    public bool? IntentToAdd { get; set; }

    [BooleanCommandSwitch("--3way")]
    public bool? ThreeWay { get; set; }

    [CommandEqualsSeparatorSwitch("--build-fake-ancestor")]
    public string? BuildFakeAncestor { get; set; }

    [BooleanCommandSwitch("--reverse")]
    public bool? Reverse { get; set; }

    [BooleanCommandSwitch("--reject")]
    public bool? Reject { get; set; }

    [BooleanCommandSwitch("--unidiff-zero")]
    public bool? UnidiffZero { get; set; }

    [BooleanCommandSwitch("--apply")]
    public bool? Apply { get; set; }

    [BooleanCommandSwitch("--no-add")]
    public bool? NoAdd { get; set; }

    [BooleanCommandSwitch("--allow-binary-replacement")]
    public bool? AllowBinaryReplacement { get; set; }

    [BooleanCommandSwitch("--binary")]
    public bool? Binary { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude")]
    public string? Exclude { get; set; }

    [CommandEqualsSeparatorSwitch("--include")]
    public string? Include { get; set; }

    [BooleanCommandSwitch("--ignore-space-change")]
    public bool? IgnoreSpaceChange { get; set; }

    [BooleanCommandSwitch("--ignore-whitespace")]
    public bool? IgnoreWhitespace { get; set; }

    [CommandEqualsSeparatorSwitch("--whitespace")]
    public string? Whitespace { get; set; }

    [BooleanCommandSwitch("--inaccurate-eof")]
    public bool? InaccurateEof { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--recount")]
    public bool? Recount { get; set; }

    [CommandEqualsSeparatorSwitch("--directory")]
    public string? Directory { get; set; }

    [BooleanCommandSwitch("--unsafe-paths")]
    public bool? UnsafePaths { get; set; }

    [BooleanCommandSwitch("--allow-empty")]
    public bool? AllowEmpty { get; set; }
}