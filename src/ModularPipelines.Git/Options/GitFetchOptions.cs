using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("fetch")]
[ExcludeFromCodeCoverage]
public record GitFetchOptions : GitOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--append")]
    public bool? Append { get; set; }

    [BooleanCommandSwitch("--atomic")]
    public bool? Atomic { get; set; }

    [CommandEqualsSeparatorSwitch("--depth")]
    public string? Depth { get; set; }

    [CommandEqualsSeparatorSwitch("--deepen")]
    public string? Deepen { get; set; }

    [CommandEqualsSeparatorSwitch("--shallow-since")]
    public string? ShallowSince { get; set; }

    [CommandEqualsSeparatorSwitch("--shallow-exclude")]
    public string? ShallowExclude { get; set; }

    [BooleanCommandSwitch("--unshallow")]
    public bool? Unshallow { get; set; }

    [BooleanCommandSwitch("--update-shallow")]
    public bool? UpdateShallow { get; set; }

    [CommandEqualsSeparatorSwitch("--negotiation-tip")]
    public string? NegotiationTip { get; set; }

    [BooleanCommandSwitch("--negotiate-only")]
    public bool? NegotiateOnly { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--porcelain")]
    public bool? Porcelain { get; set; }

    [BooleanCommandSwitch("--no-write-fetch-head")]
    public bool? NoWriteFetchHead { get; set; }

    [BooleanCommandSwitch("--write-fetch-head")]
    public bool? WriteFetchHead { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--keep")]
    public bool? Keep { get; set; }

    [BooleanCommandSwitch("--multiple")]
    public bool? Multiple { get; set; }

    [BooleanCommandSwitch("--no-auto-maintenance")]
    public bool? NoAutoMaintenance { get; set; }

    [BooleanCommandSwitch("--auto-maintenance")]
    public bool? AutoMaintenance { get; set; }

    [BooleanCommandSwitch("--no-auto-gc")]
    public bool? NoAutoGc { get; set; }

    [BooleanCommandSwitch("--auto-gc")]
    public bool? AutoGc { get; set; }

    [BooleanCommandSwitch("--no-write-commit-graph")]
    public bool? NoWriteCommitGraph { get; set; }

    [BooleanCommandSwitch("--write-commit-graph")]
    public bool? WriteCommitGraph { get; set; }

    [BooleanCommandSwitch("--prefetch")]
    public bool? Prefetch { get; set; }

    [BooleanCommandSwitch("--prune")]
    public bool? Prune { get; set; }

    [BooleanCommandSwitch("--prune-tags")]
    public bool? PruneTags { get; set; }

    [BooleanCommandSwitch("--no-tags")]
    public bool? NoTags { get; set; }

    [BooleanCommandSwitch("--refetch")]
    public bool? Refetch { get; set; }

    [CommandEqualsSeparatorSwitch("--refmap")]
    public string? Refmap { get; set; }

    [BooleanCommandSwitch("--tags")]
    public bool? Tags { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public bool? RecurseSubmodules { get; set; }

    [CommandEqualsSeparatorSwitch("--jobs")]
    public string? Jobs { get; set; }

    [BooleanCommandSwitch("--no-recurse-submodules")]
    public bool? NoRecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--set-upstream")]
    public bool? SetUpstream { get; set; }

    [CommandEqualsSeparatorSwitch("--submodule-prefix")]
    public string? SubmodulePrefix { get; set; }

    [BooleanCommandSwitch("--recurse-submodules-default")]
    public bool? RecurseSubmodulesDefault { get; set; }

    [BooleanCommandSwitch("--update-head-ok")]
    public bool? UpdateHeadOk { get; set; }

    [CommandEqualsSeparatorSwitch("--upload-pack")]
    public string? UploadPack { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--progress")]
    public bool? Progress { get; set; }

    [CommandEqualsSeparatorSwitch("--server-option")]
    public string? ServerOption { get; set; }

    [BooleanCommandSwitch("--show-forced-updates")]
    public bool? ShowForcedUpdates { get; set; }

    [BooleanCommandSwitch("--no-show-forced-updates")]
    public bool? NoShowForcedUpdates { get; set; }

    [BooleanCommandSwitch("--ipv4")]
    public bool? Ipv4 { get; set; }

    [BooleanCommandSwitch("--ipv6")]
    public bool? Ipv6 { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public bool? Stdin { get; set; }
}
