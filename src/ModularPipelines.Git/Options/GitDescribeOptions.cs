using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("describe")]
[ExcludeFromCodeCoverage]
public record GitDescribeOptions : GitOptions
{
    [CliOption("--dirty", Format = OptionFormat.EqualsSeparated)]
    public string? Dirty { get; set; }

    [CliOption("--broken", Format = OptionFormat.EqualsSeparated)]
    public string? Broken { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--tags")]
    public virtual bool? Tags { get; set; }

    [CliFlag("--contains")]
    public virtual bool? Contains { get; set; }

    [CliOption("--abbrev", Format = OptionFormat.EqualsSeparated)]
    public string? Abbrev { get; set; }

    [CliOption("--candidates", Format = OptionFormat.EqualsSeparated)]
    public string? Candidates { get; set; }

    [CliFlag("--exact-match")]
    public virtual bool? ExactMatch { get; set; }

    [CliFlag("--debug")]
    public virtual bool? Debug { get; set; }

    [CliFlag("--long")]
    public virtual bool? Long { get; set; }

    [CliOption("--match", Format = OptionFormat.EqualsSeparated)]
    public string? Match { get; set; }

    [CliOption("--exclude", Format = OptionFormat.EqualsSeparated)]
    public string? Exclude { get; set; }

    [CliFlag("--always")]
    public virtual bool? Always { get; set; }

    [CliFlag("--first-parent")]
    public virtual bool? FirstParent { get; set; }
}