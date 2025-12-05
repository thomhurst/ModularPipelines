using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("branch")]
[ExcludeFromCodeCoverage]
public record GitBranchOptions : GitOptions
{
    [CliFlag("--delete")]
    public virtual bool? Delete { get; set; }

    [CliFlag("--create-reflog")]
    public virtual bool? CreateReflog { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--move")]
    public virtual bool? Move { get; set; }

    [CliFlag("--c")]
    public virtual bool? C { get; set; }

    [CliFlag("--copy")]
    public virtual bool? Copy { get; set; }

    [CliOption("--color", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Color { get; set; }

    [CliFlag("--no-color")]
    public virtual bool? NoColor { get; set; }

    [CliFlag("--ignore-case")]
    public virtual bool? IgnoreCase { get; set; }

    [CliFlag("--omit-empty")]
    public virtual bool? OmitEmpty { get; set; }

    [CliOption("--column", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Column { get; set; }

    [CliFlag("--no-column")]
    public virtual bool? NoColumn { get; set; }

    [CliFlag("--remotes")]
    public virtual bool? Remotes { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--list")]
    public virtual bool? List { get; set; }

    [CliFlag("--show-current")]
    public virtual bool? ShowCurrent { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliOption("--abbrev", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Abbrev { get; set; }

    [CliFlag("--no-abbrev")]
    public virtual bool? NoAbbrev { get; set; }

    [CliFlag("--track")]
    public virtual bool? Track { get; set; }

    [CliFlag("--no-track")]
    public virtual bool? NoTrack { get; set; }

    [CliFlag("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [CliFlag("--set-upstream")]
    public virtual bool? SetUpstream { get; set; }

    [CliOption("--set-upstream-to", Format = OptionFormat.EqualsSeparated)]
    public virtual string? SetUpstreamTo { get; set; }

    [CliFlag("--unset-upstream")]
    public virtual bool? UnsetUpstream { get; set; }

    [CliFlag("--edit-description")]
    public virtual bool? EditDescription { get; set; }

    [CliOption("--contains", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Contains { get; set; }

    [CliOption("--no-contains", Format = OptionFormat.EqualsSeparated)]
    public virtual string? NoContains { get; set; }

    [CliOption("--merged", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Merged { get; set; }

    [CliOption("--no-merged", Format = OptionFormat.EqualsSeparated)]
    public virtual string? NoMerged { get; set; }

    [CliOption("--sort", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Sort { get; set; }

    [CliOption("--points-at", Format = OptionFormat.EqualsSeparated)]
    public virtual string? PointsAt { get; set; }

    [CliOption("--format", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Format { get; set; }
}