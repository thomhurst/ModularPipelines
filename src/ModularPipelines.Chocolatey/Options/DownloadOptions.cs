using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("download")]
public record DownloadOptions(
    [property: PositionalArgument] string Pkg
) : ChocoOptions
{
    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [CommandSwitch("--version")]
    public virtual string? Version { get; set; }

    [BooleanCommandSwitch("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [CommandSwitch("--user")]
    public virtual string? User { get; set; }

    [CommandSwitch("--password")]
    public virtual string? Password { get; set; }

    [CommandSwitch("--cert")]
    public virtual string? Cert { get; set; }

    [CommandSwitch("--certpassword")]
    public virtual string? Certpassword { get; set; }

    [CommandSwitch("--output-directory")]
    public virtual string? OutputDirectory { get; set; }

    [BooleanCommandSwitch("--ignore-dependencies")]
    public virtual bool? IgnoreDependencies { get; set; }

    [BooleanCommandSwitch("--installed-packages")]
    public virtual bool? InstalledPackages { get; set; }

    [BooleanCommandSwitch("--ignore-unfound-packages")]
    public virtual bool? IgnoreUnfoundPackages { get; set; }

    [BooleanCommandSwitch("--disable-package-repository-optimizations")]
    public virtual bool? DisablePackageRepositoryOptimizations { get; set; }

    [BooleanCommandSwitch("--internalize")]
    public virtual bool? Internalize { get; set; }

    [CommandSwitch("--resources-location")]
    public virtual string? ResourcesLocation { get; set; }

    [CommandSwitch("--download-location")]
    public virtual string? DownloadLocation { get; set; }

    [BooleanCommandSwitch("--internalize-all-urls")]
    public virtual bool? InternalizeAllUrls { get; set; }

    [BooleanCommandSwitch("--append-use-original-location")]
    public virtual bool? AppendUseOriginalLocation { get; set; }

    [BooleanCommandSwitch("--skip-download-cache")]
    public virtual bool? SkipDownloadCache { get; set; }

    [BooleanCommandSwitch("--use-download-cache")]
    public virtual bool? UseDownloadCache { get; set; }

    [BooleanCommandSwitch("--skip-virus-check")]
    public virtual bool? SkipVirusCheck { get; set; }

    [BooleanCommandSwitch("--virus-check")]
    public virtual bool? VirusCheck { get; set; }

    [CommandSwitch("--virus-positives-minimum")]
    public virtual string? VirusPositivesMinimum { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}