using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("ls-tree")]
[ExcludeFromCodeCoverage]
public record GitLsTreeOptions : GitOptions
{
    [CliFlag("--long")]
    public virtual bool? Long { get; set; }

    [CliFlag("--name-only")]
    public virtual bool? NameOnly { get; set; }

    [CliFlag("--name-status")]
    public virtual bool? NameStatus { get; set; }

    [CliFlag("--object-only")]
    public virtual bool? ObjectOnly { get; set; }

    [CliOption("--abbrev", Format = OptionFormat.EqualsSeparated)]
    public string? Abbrev { get; set; }

    [CliFlag("--full-name")]
    public virtual bool? FullName { get; set; }

    [CliFlag("--full-tree")]
    public virtual bool? FullTree { get; set; }

    [CliOption("--format", Format = OptionFormat.EqualsSeparated)]
    public string? Format { get; set; }
}