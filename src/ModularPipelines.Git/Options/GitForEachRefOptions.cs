using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("for-each-ref")]
[ExcludeFromCodeCoverage]
public record GitForEachRefOptions : GitOptions
{
    [CliFlag("--stdin")]
    public virtual bool? Stdin { get; set; }

    [CliOption("--count", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Count { get; set; }

    [CliOption("--sort", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Sort { get; set; }

    [CliOption("--format", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Format { get; set; }

    [CliOption("--color", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Color { get; set; }

    [CliFlag("--shell")]
    public virtual bool? Shell { get; set; }

    [CliFlag("--perl")]
    public virtual bool? Perl { get; set; }

    [CliFlag("--python")]
    public virtual bool? Python { get; set; }

    [CliFlag("--tcl")]
    public virtual bool? Tcl { get; set; }

    [CliOption("--points-at", Format = OptionFormat.EqualsSeparated)]
    public virtual string? PointsAt { get; set; }

    [CliOption("--merged", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Merged { get; set; }

    [CliOption("--no-merged", Format = OptionFormat.EqualsSeparated)]
    public virtual string? NoMerged { get; set; }

    [CliOption("--contains", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Contains { get; set; }

    [CliOption("--no-contains", Format = OptionFormat.EqualsSeparated)]
    public virtual string? NoContains { get; set; }

    [CliFlag("--ignore-case")]
    public virtual bool? IgnoreCase { get; set; }

    [CliFlag("--omit-empty")]
    public virtual bool? OmitEmpty { get; set; }
}