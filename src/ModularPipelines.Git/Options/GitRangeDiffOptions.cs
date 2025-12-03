using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("range-diff")]
[ExcludeFromCodeCoverage]
public record GitRangeDiffOptions : GitOptions
{
    [CliFlag("--no-dual-color")]
    public virtual bool? NoDualColor { get; set; }

    [CliOption("--creation-factor", Format = OptionFormat.EqualsSeparated)]
    public string? CreationFactor { get; set; }

    [CliFlag("--left-only")]
    public virtual bool? LeftOnly { get; set; }

    [CliFlag("--right-only")]
    public virtual bool? RightOnly { get; set; }

    [CliOption("--no-notes", Format = OptionFormat.EqualsSeparated)]
    public string? NoNotes { get; set; }

    [CliOption("--notes", Format = OptionFormat.EqualsSeparated)]
    public string? Notes { get; set; }
}