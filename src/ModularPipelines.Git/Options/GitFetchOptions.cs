using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("fetch")]
[ExcludeFromCodeCoverage]
public record GitFetchOptions : GitOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--append")]
    public virtual bool? Append { get; set; }

    [CliFlag("--atomic")]
    public virtual bool? Atomic { get; set; }

    [CliOption("--depth", Format = OptionFormat.EqualsSeparated)]
    public string? Depth { get; set; }

    [CliOption("--deepen", Format = OptionFormat.EqualsSeparated)]
    public string? Deepen { get; set; }

    [CliOption("--shallow-since", Format = OptionFormat.EqualsSeparated)]
    public string? ShallowSince { get; set; }

    [CliOption("--shallow-exclude", Format = OptionFormat.EqualsSeparated)]
    public string? ShallowExclude { get; set; }

    [CliFlag("--unshallow")]
    public virtual bool? Unshallow { get; set; }

    [CliFlag("--update-shallow")]
    public virtual bool? UpdateShallow { get; set; }

    [CliOption("--negotiation-tip", Format = OptionFormat.EqualsSeparated)]
    public string? NegotiationTip { get; set; }

    [CliFlag("--negotiate-only")]
    public virtual bool? NegotiateOnly { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--porcelain")]
    public virtual bool? Porcelain { get; set; }

    [CliFlag("--no-write-fetch-head")]
    public virtual bool? NoWriteFetchHead { get; set; }

    [CliFlag("--write-fetch-head")]
    public virtual bool? WriteFetchHead { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--keep")]
    public virtual bool? Keep { get; set; }

    [CliFlag("--multiple")]
    public virtual bool? Multiple { get; set; }

    [CliFlag("--no-auto-maintenance")]
    public virtual bool? NoAutoMaintenance { get; set; }

    [CliFlag("--auto-maintenance")]
    public virtual bool? AutoMaintenance { get; set; }

    [CliFlag("--no-auto-gc")]
    public virtual bool? NoAutoGc { get; set; }

    [CliFlag("--auto-gc")]
    public virtual bool? AutoGc { get; set; }

    [CliFlag("--no-write-commit-graph")]
    public virtual bool? NoWriteCommitGraph { get; set; }

    [CliFlag("--write-commit-graph")]
    public virtual bool? WriteCommitGraph { get; set; }

    [CliFlag("--prefetch")]
    public virtual bool? Prefetch { get; set; }

    [CliFlag("--prune")]
    public virtual bool? Prune { get; set; }

    [CliFlag("--prune-tags")]
    public virtual bool? PruneTags { get; set; }

    [CliFlag("--no-tags")]
    public virtual bool? NoTags { get; set; }

    [CliFlag("--refetch")]
    public virtual bool? Refetch { get; set; }

    [CliOption("--refmap", Format = OptionFormat.EqualsSeparated)]
    public string? Refmap { get; set; }

    [CliFlag("--tags")]
    public virtual bool? Tags { get; set; }

    [CliFlag("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [CliOption("--jobs", Format = OptionFormat.EqualsSeparated)]
    public string? Jobs { get; set; }

    [CliFlag("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }

    [CliFlag("--set-upstream")]
    public virtual bool? SetUpstream { get; set; }

    [CliOption("--submodule-prefix", Format = OptionFormat.EqualsSeparated)]
    public string? SubmodulePrefix { get; set; }

    [CliFlag("--recurse-submodules-default")]
    public virtual bool? RecurseSubmodulesDefault { get; set; }

    [CliFlag("--update-head-ok")]
    public virtual bool? UpdateHeadOk { get; set; }

    [CliOption("--upload-pack", Format = OptionFormat.EqualsSeparated)]
    public string? UploadPack { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--progress")]
    public virtual bool? Progress { get; set; }

    [CliOption("--server-option", Format = OptionFormat.EqualsSeparated)]
    public string? ServerOption { get; set; }

    [CliFlag("--show-forced-updates")]
    public virtual bool? ShowForcedUpdates { get; set; }

    [CliFlag("--no-show-forced-updates")]
    public virtual bool? NoShowForcedUpdates { get; set; }

    [CliFlag("--ipv4")]
    public virtual bool? Ipv4 { get; set; }

    [CliFlag("--ipv6")]
    public virtual bool? Ipv6 { get; set; }

    [CliFlag("--stdin")]
    public virtual bool? Stdin { get; set; }
}