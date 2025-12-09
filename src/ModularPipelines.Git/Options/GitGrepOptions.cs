using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("grep")]
[ExcludeFromCodeCoverage]
public record GitGrepOptions : GitOptions
{
    [CliFlag("--cached")]
    public virtual bool? Cached { get; set; }

    [CliFlag("--no-index")]
    public virtual bool? NoIndex { get; set; }

    [CliFlag("--untracked")]
    public virtual bool? Untracked { get; set; }

    [CliFlag("--no-exclude-standard")]
    public virtual bool? NoExcludeStandard { get; set; }

    [CliFlag("--exclude-standard")]
    public virtual bool? ExcludeStandard { get; set; }

    [CliFlag("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [CliFlag("--text")]
    public virtual bool? Text { get; set; }

    [CliFlag("--textconv")]
    public virtual bool? Textconv { get; set; }

    [CliFlag("--no-textconv")]
    public virtual bool? NoTextconv { get; set; }

    [CliFlag("--ignore-case")]
    public virtual bool? IgnoreCase { get; set; }

    [CliOption("--max-depth", Format = OptionFormat.EqualsSeparated)]
    public virtual string? MaxDepth { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliFlag("--no-recursive")]
    public virtual bool? NoRecursive { get; set; }

    [CliFlag("--word-regexp")]
    public virtual bool? WordRegexp { get; set; }

    [CliFlag("--invert-match")]
    public virtual bool? InvertMatch { get; set; }

    [CliFlag("--full-name")]
    public virtual bool? FullName { get; set; }

    [CliFlag("--extended-regexp")]
    public virtual bool? ExtendedRegexp { get; set; }

    [CliFlag("--basic-regexp")]
    public virtual bool? BasicRegexp { get; set; }

    [CliFlag("--perl-regexp")]
    public virtual bool? PerlRegexp { get; set; }

    [CliFlag("--fixed-strings")]
    public virtual bool? FixedStrings { get; set; }

    [CliFlag("--line-number")]
    public virtual bool? LineNumber { get; set; }

    [CliFlag("--column")]
    public virtual bool? Column { get; set; }

    [CliFlag("--files-with-matches")]
    public virtual bool? FilesWithMatches { get; set; }

    [CliFlag("--name-only")]
    public virtual bool? NameOnly { get; set; }

    [CliFlag("--files-without-match")]
    public virtual bool? FilesWithoutMatch { get; set; }

    [CliOption("--open-files-in-pager", Format = OptionFormat.EqualsSeparated)]
    public virtual string? OpenFilesInPager { get; set; }

    [CliFlag("--null")]
    public virtual bool? Null { get; set; }

    [CliFlag("--only-matching")]
    public virtual bool? OnlyMatching { get; set; }

    [CliFlag("--c")]
    public virtual bool? C { get; set; }

    [CliFlag("--count")]
    public virtual bool? Count { get; set; }

    [CliOption("--color", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Color { get; set; }

    [CliFlag("--no-color")]
    public virtual bool? NoColor { get; set; }

    [CliFlag("--break")]
    public virtual bool? Break { get; set; }

    [CliFlag("--heading")]
    public virtual bool? Heading { get; set; }

    [CliFlag("--show-function")]
    public virtual bool? ShowFunction { get; set; }

    [CliOption("--context", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Context { get; set; }

    [CliOption("--after-context", Format = OptionFormat.EqualsSeparated)]
    public virtual string? AfterContext { get; set; }

    [CliOption("--before-context", Format = OptionFormat.EqualsSeparated)]
    public virtual string? BeforeContext { get; set; }

    [CliFlag("--function-context")]
    public virtual bool? FunctionContext { get; set; }

    [CliOption("--max-count", Format = OptionFormat.EqualsSeparated)]
    public virtual string? MaxCount { get; set; }

    [CliOption("--threads", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Threads { get; set; }

    [CliFlag("--and")]
    public virtual bool? And { get; set; }

    [CliFlag("--or")]
    public virtual bool? Or { get; set; }

    [CliFlag("--not")]
    public virtual bool? Not { get; set; }

    [CliFlag("--all-match")]
    public virtual bool? AllMatch { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}