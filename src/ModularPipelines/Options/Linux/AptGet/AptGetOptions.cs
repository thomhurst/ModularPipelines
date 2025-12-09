using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public record AptGetOptions : CommandLineToolOptions
{
    public AptGetOptions() : base("apt-get")
    {
        Sudo = true;
    }

    [CliFlag("--download-only")]
    public virtual bool? DownloadOnly { get; set; }

    [CliFlag("--fix-broken")]
    public virtual bool? FixBroken { get; set; }

    [CliFlag("--fix-missing")]
    public virtual bool? FixMissing { get; set; }

    [CliFlag("--no-download")]
    public virtual bool? NoDownload { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--assume-yes")]
    public virtual bool? AssumeYes { get; set; } = true;

    [CliFlag("--show-upgraded")]
    public virtual bool? ShowUpgraded { get; set; }

    [CliFlag("--verbose-versions")]
    public virtual bool? VerboseVersions { get; set; }

    [CliFlag("--build")]
    public virtual bool? Build { get; set; }

    [CliFlag("--ignore-hold")]
    public virtual bool? IgnoreHold { get; set; }

    [CliFlag("--no-upgrade")]
    public virtual bool? NoUpgrade { get; set; }

    [CliFlag("--force-yes")]
    public virtual bool? ForceYes { get; set; }

    [CliFlag("--print-uris")]
    public virtual bool? PrintUris { get; set; }

    [CliFlag("--reinstall")]
    public virtual bool? Reinstall { get; set; }

    [CliFlag("--list-cleanup")]
    public virtual bool? ListCleanup { get; set; }

    [CliFlag("--default-release")]
    public virtual bool? DefaultRelease { get; set; }

    [CliFlag("--trivial-only")]
    public virtual bool? TrivialOnly { get; set; }

    [CliFlag("--no-remove")]
    public virtual bool? NoRemove { get; set; }

    [CliFlag("--only-source")]
    public virtual bool? OnlySource { get; set; }

    [CliFlag("--help")]
    public virtual bool? Help { get; set; }

    [CliFlag("--version")]
    public virtual bool? Version { get; set; }

    [CliFlag("--config-file")]
    public virtual bool? ConfigFile { get; set; }

    [CliFlag("--option")]
    public virtual bool? Option { get; set; }
}