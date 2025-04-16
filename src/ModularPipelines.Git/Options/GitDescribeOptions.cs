using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("describe")]
[ExcludeFromCodeCoverage]
public record GitDescribeOptions : GitOptions
{
    [CommandEqualsSeparatorSwitch("--dirty")]
    public string? Dirty { get; set; }

    [CommandEqualsSeparatorSwitch("--broken")]
    public string? Broken { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--tags")]
    public virtual bool? Tags { get; set; }

    [BooleanCommandSwitch("--contains")]
    public virtual bool? Contains { get; set; }

    [CommandEqualsSeparatorSwitch("--abbrev")]
    public string? Abbrev { get; set; }

    [CommandEqualsSeparatorSwitch("--candidates")]
    public string? Candidates { get; set; }

    [BooleanCommandSwitch("--exact-match")]
    public virtual bool? ExactMatch { get; set; }

    [BooleanCommandSwitch("--debug")]
    public virtual bool? Debug { get; set; }

    [BooleanCommandSwitch("--long")]
    public virtual bool? Long { get; set; }

    [CommandEqualsSeparatorSwitch("--match")]
    public string? Match { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude")]
    public string? Exclude { get; set; }

    [BooleanCommandSwitch("--always")]
    public virtual bool? Always { get; set; }

    [BooleanCommandSwitch("--first-parent")]
    public virtual bool? FirstParent { get; set; }
}