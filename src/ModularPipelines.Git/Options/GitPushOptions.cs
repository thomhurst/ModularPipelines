using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("push")]
[ExcludeFromCodeCoverage]
public record GitPushOptions : GitOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--branches")]
    public virtual bool? Branches { get; set; }

    [CliFlag("--prune")]
    public virtual bool? Prune { get; set; }

    [CliFlag("--mirror")]
    public virtual bool? Mirror { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--porcelain")]
    public virtual bool? Porcelain { get; set; }

    [CliFlag("--delete")]
    public virtual bool? Delete { get; set; }

    [CliFlag("--tags")]
    public virtual bool? Tags { get; set; }

    [CliFlag("--follow-tags")]
    public virtual bool? FollowTags { get; set; }

    [CliFlag("--no-signed")]
    public virtual bool? NoSigned { get; set; }

    [CliFlag("--signed")]
    public virtual bool? Signed { get; set; }

    [CliFlag("--no-atomic")]
    public virtual bool? NoAtomic { get; set; }

    [CliFlag("--atomic")]
    public virtual bool? Atomic { get; set; }

    [CliOption("--push-option", Format = OptionFormat.EqualsSeparated)]
    public string? PushOption { get; set; }

    [CliOption("--receive-pack", Format = OptionFormat.EqualsSeparated)]
    public string? ReceivePack { get; set; }

    [CliOption("--exec", Format = OptionFormat.EqualsSeparated)]
    public string? Exec { get; set; }

    [CliFlag("--no-force-with-lease")]
    public virtual bool? NoForceWithLease { get; set; }

    [CliFlag("--force-with-lease")]
    public virtual bool? ForceWithLease { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--no-force-if-includes")]
    public virtual bool? NoForceIfIncludes { get; set; }

    [CliFlag("--force-if-includes")]
    public virtual bool? ForceIfIncludes { get; set; }

    [CliOption("--repo", Format = OptionFormat.EqualsSeparated)]
    public string? Repo { get; set; }

    [CliFlag("--set-upstream")]
    public virtual bool? SetUpstream { get; set; }

    [CliFlag("--no-thin")]
    public virtual bool? NoThin { get; set; }

    [CliFlag("--thin")]
    public virtual bool? Thin { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--progress")]
    public virtual bool? Progress { get; set; }

    [CliFlag("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }

    [CliFlag("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [CliFlag("--no-verify")]
    public virtual bool? NoVerify { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }

    [CliFlag("--ipv4")]
    public virtual bool? Ipv4 { get; set; }

    [CliFlag("--ipv6")]
    public virtual bool? Ipv6 { get; set; }
}