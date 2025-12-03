using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("tag")]
[ExcludeFromCodeCoverage]
public record GitTagOptions : GitOptions
{
    [CliFlag("--annotate")]
    public virtual bool? Annotate { get; set; }

    [CliFlag("--sign")]
    public virtual bool? Sign { get; set; }

    [CliFlag("--no-sign")]
    public virtual bool? NoSign { get; set; }

    [CliOption("--local-user", Format = OptionFormat.EqualsSeparated)]
    public string? LocalUser { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--delete")]
    public virtual bool? Delete { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }

    [CliFlag("--list")]
    public virtual bool? List { get; set; }

    [CliOption("--sort", Format = OptionFormat.EqualsSeparated)]
    public string? Sort { get; set; }

    [CliOption("--color", Format = OptionFormat.EqualsSeparated)]
    public string? Color { get; set; }

    [CliFlag("--ignore-case")]
    public virtual bool? IgnoreCase { get; set; }

    [CliFlag("--omit-empty")]
    public virtual bool? OmitEmpty { get; set; }

    [CliOption("--column", Format = OptionFormat.EqualsSeparated)]
    public string? Column { get; set; }

    [CliFlag("--no-column")]
    public virtual bool? NoColumn { get; set; }

    [CliOption("--contains", Format = OptionFormat.EqualsSeparated)]
    public string? Contains { get; set; }

    [CliOption("--no-contains", Format = OptionFormat.EqualsSeparated)]
    public string? NoContains { get; set; }

    [CliOption("--merged", Format = OptionFormat.EqualsSeparated)]
    public string? Merged { get; set; }

    [CliOption("--no-merged", Format = OptionFormat.EqualsSeparated)]
    public string? NoMerged { get; set; }

    [CliOption("--points-at", Format = OptionFormat.EqualsSeparated)]
    public string? PointsAt { get; set; }

    [CliOption("--message", Format = OptionFormat.EqualsSeparated)]
    public string? Message { get; set; }

    [CliOption("--file", Format = OptionFormat.EqualsSeparated)]
    public string? File { get; set; }

    [CliFlag("--edit")]
    public virtual bool? Edit { get; set; }

    [CliOption("--cleanup", Format = OptionFormat.EqualsSeparated)]
    public string? Cleanup { get; set; }

    [CliFlag("--create-reflog")]
    public virtual bool? CreateReflog { get; set; }

    [CliOption("--format", Format = OptionFormat.EqualsSeparated)]
    public string? Format { get; set; }
}