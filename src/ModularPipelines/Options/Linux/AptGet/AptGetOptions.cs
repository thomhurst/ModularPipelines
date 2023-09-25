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
    public bool? DownloadOnly { get; set; }

    [BooleanCommandSwitch("--fix-broken")]
    public bool? FixBroken { get; set; }

    [BooleanCommandSwitch("--fix-missing")]
    public bool? FixMissing { get; set; }

    [BooleanCommandSwitch("--no-download")]
    public bool? NoDownload { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--assume-yes")]
    public bool? AssumeYes { get; set; } = true;

    [BooleanCommandSwitch("--show-upgraded")]
    public bool? ShowUpgraded { get; set; }

    [BooleanCommandSwitch("--verbose-versions")]
    public bool? VerboseVersions { get; set; }

    [BooleanCommandSwitch("--build")]
    public bool? Build { get; set; }

    [BooleanCommandSwitch("--ignore-hold")]
    public bool? IgnoreHold { get; set; }

    [BooleanCommandSwitch("--no-upgrade")]
    public bool? NoUpgrade { get; set; }

    [BooleanCommandSwitch("--force-yes")]
    public bool? ForceYes { get; set; }

    [BooleanCommandSwitch("--print-uris")]
    public bool? PrintUris { get; set; }

    [BooleanCommandSwitch("--reinstall")]
    public bool? Reinstall { get; set; }

    [BooleanCommandSwitch("--list-cleanup")]
    public bool? ListCleanup { get; set; }

    [BooleanCommandSwitch("--default-release")]
    public bool? DefaultRelease { get; set; }

    [BooleanCommandSwitch("--trivial-only")]
    public bool? TrivialOnly { get; set; }

    [BooleanCommandSwitch("--no-remove")]
    public bool? NoRemove { get; set; }

    [BooleanCommandSwitch("--only-source")]
    public bool? OnlySource { get; set; }

    [BooleanCommandSwitch("--help")]
    public bool? Help { get; set; }

    [BooleanCommandSwitch("--version")]
    public bool? Version { get; set; }

    [BooleanCommandSwitch("--config-file")]
    public bool? ConfigFile { get; set; }

    [BooleanCommandSwitch("--option")]
    public bool? Option { get; set; }
}
