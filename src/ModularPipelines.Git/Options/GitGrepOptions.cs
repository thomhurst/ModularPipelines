using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("grep")]
[ExcludeFromCodeCoverage]
public record GitGrepOptions : GitOptions
{
    [BooleanCommandSwitch("--cached")]
    public virtual bool? Cached { get; set; }

    [BooleanCommandSwitch("--no-index")]
    public virtual bool? NoIndex { get; set; }

    [BooleanCommandSwitch("--untracked")]
    public virtual bool? Untracked { get; set; }

    [BooleanCommandSwitch("--no-exclude-standard")]
    public virtual bool? NoExcludeStandard { get; set; }

    [BooleanCommandSwitch("--exclude-standard")]
    public virtual bool? ExcludeStandard { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--text")]
    public virtual bool? Text { get; set; }

    [BooleanCommandSwitch("--textconv")]
    public virtual bool? Textconv { get; set; }

    [BooleanCommandSwitch("--no-textconv")]
    public virtual bool? NoTextconv { get; set; }

    [BooleanCommandSwitch("--ignore-case")]
    public virtual bool? IgnoreCase { get; set; }

    [CommandEqualsSeparatorSwitch("--max-depth")]
    public string? MaxDepth { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public virtual bool? Recursive { get; set; }

    [BooleanCommandSwitch("--no-recursive")]
    public virtual bool? NoRecursive { get; set; }

    [BooleanCommandSwitch("--word-regexp")]
    public virtual bool? WordRegexp { get; set; }

    [BooleanCommandSwitch("--invert-match")]
    public virtual bool? InvertMatch { get; set; }

    [BooleanCommandSwitch("--full-name")]
    public virtual bool? FullName { get; set; }

    [BooleanCommandSwitch("--extended-regexp")]
    public virtual bool? ExtendedRegexp { get; set; }

    [BooleanCommandSwitch("--basic-regexp")]
    public virtual bool? BasicRegexp { get; set; }

    [BooleanCommandSwitch("--perl-regexp")]
    public virtual bool? PerlRegexp { get; set; }

    [BooleanCommandSwitch("--fixed-strings")]
    public virtual bool? FixedStrings { get; set; }

    [BooleanCommandSwitch("--line-number")]
    public virtual bool? LineNumber { get; set; }

    [BooleanCommandSwitch("--column")]
    public virtual bool? Column { get; set; }

    [BooleanCommandSwitch("--files-with-matches")]
    public virtual bool? FilesWithMatches { get; set; }

    [BooleanCommandSwitch("--name-only")]
    public virtual bool? NameOnly { get; set; }

    [BooleanCommandSwitch("--files-without-match")]
    public virtual bool? FilesWithoutMatch { get; set; }

    [CommandEqualsSeparatorSwitch("--open-files-in-pager")]
    public string? OpenFilesInPager { get; set; }

    [BooleanCommandSwitch("--null")]
    public virtual bool? Null { get; set; }

    [BooleanCommandSwitch("--only-matching")]
    public virtual bool? OnlyMatching { get; set; }

    [BooleanCommandSwitch("--c")]
    public virtual bool? C { get; set; }

    [BooleanCommandSwitch("--count")]
    public virtual bool? Count { get; set; }

    [CommandEqualsSeparatorSwitch("--color")]
    public string? Color { get; set; }

    [BooleanCommandSwitch("--no-color")]
    public virtual bool? NoColor { get; set; }

    [BooleanCommandSwitch("--break")]
    public virtual bool? Break { get; set; }

    [BooleanCommandSwitch("--heading")]
    public virtual bool? Heading { get; set; }

    [BooleanCommandSwitch("--show-function")]
    public virtual bool? ShowFunction { get; set; }

    [CommandEqualsSeparatorSwitch("--context")]
    public string? Context { get; set; }

    [CommandEqualsSeparatorSwitch("--after-context")]
    public string? AfterContext { get; set; }

    [CommandEqualsSeparatorSwitch("--before-context")]
    public string? BeforeContext { get; set; }

    [BooleanCommandSwitch("--function-context")]
    public virtual bool? FunctionContext { get; set; }

    [CommandEqualsSeparatorSwitch("--max-count")]
    public string? MaxCount { get; set; }

    [CommandEqualsSeparatorSwitch("--threads")]
    public string? Threads { get; set; }

    [BooleanCommandSwitch("--and")]
    public virtual bool? And { get; set; }

    [BooleanCommandSwitch("--or")]
    public virtual bool? Or { get; set; }

    [BooleanCommandSwitch("--not")]
    public virtual bool? Not { get; set; }

    [BooleanCommandSwitch("--all-match")]
    public virtual bool? AllMatch { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}