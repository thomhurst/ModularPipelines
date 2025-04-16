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

    [BooleanCommandSwitch("--download-only")]
    public virtual bool? DownloadOnly { get; set; }

    [BooleanCommandSwitch("--fix-broken")]
    public virtual bool? FixBroken { get; set; }

    [BooleanCommandSwitch("--fix-missing")]
    public virtual bool? FixMissing { get; set; }

    [BooleanCommandSwitch("--no-download")]
    public virtual bool? NoDownload { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--assume-yes")]
    public virtual bool? AssumeYes { get; set; } = true;

    [BooleanCommandSwitch("--show-upgraded")]
    public virtual bool? ShowUpgraded { get; set; }

    [BooleanCommandSwitch("--verbose-versions")]
    public virtual bool? VerboseVersions { get; set; }

    [BooleanCommandSwitch("--build")]
    public virtual bool? Build { get; set; }

    [BooleanCommandSwitch("--ignore-hold")]
    public virtual bool? IgnoreHold { get; set; }

    [BooleanCommandSwitch("--no-upgrade")]
    public virtual bool? NoUpgrade { get; set; }

    [BooleanCommandSwitch("--force-yes")]
    public virtual bool? ForceYes { get; set; }

    [BooleanCommandSwitch("--print-uris")]
    public virtual bool? PrintUris { get; set; }

    [BooleanCommandSwitch("--reinstall")]
    public virtual bool? Reinstall { get; set; }

    [BooleanCommandSwitch("--list-cleanup")]
    public virtual bool? ListCleanup { get; set; }

    [BooleanCommandSwitch("--default-release")]
    public virtual bool? DefaultRelease { get; set; }

    [BooleanCommandSwitch("--trivial-only")]
    public virtual bool? TrivialOnly { get; set; }

    [BooleanCommandSwitch("--no-remove")]
    public virtual bool? NoRemove { get; set; }

    [BooleanCommandSwitch("--only-source")]
    public virtual bool? OnlySource { get; set; }

    [BooleanCommandSwitch("--help")]
    public virtual bool? Help { get; set; }

    [BooleanCommandSwitch("--version")]
    public virtual bool? Version { get; set; }

    [BooleanCommandSwitch("--config-file")]
    public virtual bool? ConfigFile { get; set; }

    [BooleanCommandSwitch("--option")]
    public virtual bool? Option { get; set; }
}