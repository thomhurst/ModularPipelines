using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("fetch")]
[ExcludeFromCodeCoverage]
public record GitFetchOptions : GitOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--append")]
    public virtual bool? Append { get; set; }

    [BooleanCommandSwitch("--atomic")]
    public virtual bool? Atomic { get; set; }

    [CommandEqualsSeparatorSwitch("--depth")]
    public string? Depth { get; set; }

    [CommandEqualsSeparatorSwitch("--deepen")]
    public string? Deepen { get; set; }

    [CommandEqualsSeparatorSwitch("--shallow-since")]
    public string? ShallowSince { get; set; }

    [CommandEqualsSeparatorSwitch("--shallow-exclude")]
    public string? ShallowExclude { get; set; }

    [BooleanCommandSwitch("--unshallow")]
    public virtual bool? Unshallow { get; set; }

    [BooleanCommandSwitch("--update-shallow")]
    public virtual bool? UpdateShallow { get; set; }

    [CommandEqualsSeparatorSwitch("--negotiation-tip")]
    public string? NegotiationTip { get; set; }

    [BooleanCommandSwitch("--negotiate-only")]
    public virtual bool? NegotiateOnly { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--porcelain")]
    public virtual bool? Porcelain { get; set; }

    [BooleanCommandSwitch("--no-write-fetch-head")]
    public virtual bool? NoWriteFetchHead { get; set; }

    [BooleanCommandSwitch("--write-fetch-head")]
    public virtual bool? WriteFetchHead { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--keep")]
    public virtual bool? Keep { get; set; }

    [BooleanCommandSwitch("--multiple")]
    public virtual bool? Multiple { get; set; }

    [BooleanCommandSwitch("--no-auto-maintenance")]
    public virtual bool? NoAutoMaintenance { get; set; }

    [BooleanCommandSwitch("--auto-maintenance")]
    public virtual bool? AutoMaintenance { get; set; }

    [BooleanCommandSwitch("--no-auto-gc")]
    public virtual bool? NoAutoGc { get; set; }

    [BooleanCommandSwitch("--auto-gc")]
    public virtual bool? AutoGc { get; set; }

    [BooleanCommandSwitch("--no-write-commit-graph")]
    public virtual bool? NoWriteCommitGraph { get; set; }

    [BooleanCommandSwitch("--write-commit-graph")]
    public virtual bool? WriteCommitGraph { get; set; }

    [BooleanCommandSwitch("--prefetch")]
    public virtual bool? Prefetch { get; set; }

    [BooleanCommandSwitch("--prune")]
    public virtual bool? Prune { get; set; }

    [BooleanCommandSwitch("--prune-tags")]
    public virtual bool? PruneTags { get; set; }

    [BooleanCommandSwitch("--no-tags")]
    public virtual bool? NoTags { get; set; }

    [BooleanCommandSwitch("--refetch")]
    public virtual bool? Refetch { get; set; }

    [CommandEqualsSeparatorSwitch("--refmap")]
    public string? Refmap { get; set; }

    [BooleanCommandSwitch("--tags")]
    public virtual bool? Tags { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [CommandEqualsSeparatorSwitch("--jobs")]
    public string? Jobs { get; set; }

    [BooleanCommandSwitch("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--set-upstream")]
    public virtual bool? SetUpstream { get; set; }

    [CommandEqualsSeparatorSwitch("--submodule-prefix")]
    public string? SubmodulePrefix { get; set; }

    [BooleanCommandSwitch("--recurse-submodules-default")]
    public virtual bool? RecurseSubmodulesDefault { get; set; }

    [BooleanCommandSwitch("--update-head-ok")]
    public virtual bool? UpdateHeadOk { get; set; }

    [CommandEqualsSeparatorSwitch("--upload-pack")]
    public string? UploadPack { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--progress")]
    public virtual bool? Progress { get; set; }

    [CommandEqualsSeparatorSwitch("--server-option")]
    public string? ServerOption { get; set; }

    [BooleanCommandSwitch("--show-forced-updates")]
    public virtual bool? ShowForcedUpdates { get; set; }

    [BooleanCommandSwitch("--no-show-forced-updates")]
    public virtual bool? NoShowForcedUpdates { get; set; }

    [BooleanCommandSwitch("--ipv4")]
    public virtual bool? Ipv4 { get; set; }

    [BooleanCommandSwitch("--ipv6")]
    public virtual bool? Ipv6 { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public virtual bool? Stdin { get; set; }
}